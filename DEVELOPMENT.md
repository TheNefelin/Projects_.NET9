# DEVELOPMENT - WebApiCore (migración)

Documento de decisiones y avance de la migración de `WebApi`/`ProjectAuth`/`ProjectPasswordManager` hacia una única API `WebApiCore` (.NET 9) con Clean Architecture y SOLID.

## Estado

- [x] Fase 1: estructura de proyectos, migración de código (Domain, Application, Infrastructure, host) y build en verde.
- [x] Fase 2: tests de integración contra `db_testing` (Docker). Ejecutados: 20/20 superados.
- [x] Fase 3: verificación en runtime. Flujo completo verificado contra `db_testing` (register → login → register-password → get-iv → CRUD /api/core/*) con ApiKey y JWT.

Los proyectos originales (`WebApi`, `ProjectAuth`, `ProjectPasswordManager`, etc.) se conservan intactos durante la migración.

## Arquitectura

### Grafo de referencias

Las dependencias apuntan hacia adentro (Domain no depende de nada):

```
WebApiCore ──┬──> WebApiCore.Application ──> WebApiCore.Domain
             └──> WebApiCore.Infrastructure ──> WebApiCore.Domain
                                     └──> WebApiCore.Application
WebApiCore.Tests ──> WebApiCore.Application
                 └──> WebApiCore.Infrastructure
```

- **Domain**: entidades, interfaces de repositorio y modelos (sin dependencias).
- **Application**: DTOs, `ApiResponse<T>`, interfaces de servicios y casos de uso.
- **Infrastructure**: `DapperContext`, repositorios, `PasswordHasher`, `JwtTokenUtil`, `JwtOptions`.
- **WebApiCore**: composition root (DI), controllers, filters, middleware.

## Decisiones técnicas

### 1. Envelope de respuesta único: `ApiResponse<T>` (en vez de ProblemDetails)

**Decisión**: mantener `ApiResponse<T>` como formato único para éxito y error, y estandarizar todos los caminos de error hacia ese envelope (validación de modelo, JWT 401, 404 y excepciones).

**Motivación**: los clientes actuales ya consumen `isSuccess/statusCode/message/data`. Adoptar ProblemDetails (RFC 9457) habría introducido un segundo formato de error, agravando la inconsistencia que motivó el refactor. ProblemDetails queda descartado salvo una futura v2 deliberadamente breaking.

**Implementación de la estandarización**:
- Validación de modelo → `InvalidModelStateResponseFactory` devuelve `ApiResponse` 400 con `Errors`.
- JWT 401 → `JwtBearerEvents.OnChallenge` devuelve `ApiResponse` 401.
- 404 de rutas no existentes → `MapFallback` devuelve `ApiResponse` 404.
- Excepciones no controladas → `IExceptionHandler` (`GlobalExceptionHandler`) devuelve `ApiResponse` con status según tipo y `TraceId`.

### 2. Los servicios no tragan excepciones

**Decisión**: eliminar los `try/catch` que envolvían cada operación y devolvían 500 con `ex.Message`/`ex.ToString()`.

**Motivación**: fuga de detalles internos y el `GlobalExceptionHandler` del proyecto original nunca se ejecutaba porque las excepciones se capturaban en la capa de servicios. Ahora los errores inesperados se propagan al `IExceptionHandler`, que responde con mensajes genéricos y registra la excepción real en logs.

### 3. DIP: dependencias por interfaz

**Decisión**: `PasswordUtil` y `JwtTokenUtil` (concretas) se sustituyen por las interfaces `IPasswordHasher` e `IAuthTokenService` definidas en Application e implementadas en Infrastructure.

**Motivación**: los casos de uso ya no dependen de implementaciones concretas de seguridad; se pueden intercambiar sin tocar Application.

### 4. Configuración JWT con Options pattern

**Decisión**: la sección `JWT` de `appsettings.json` se vincula al POCO `JwtOptions` (Infrastructure) y se registra como singleton en el host.

**Motivación**: elimina la construcción manual de `JwtConfig` en el controller y la mezcla de claves de configuración (`Jwt:` vs `JWT:`) que existía en el original. Se registra la instancia como singleton para evitar agregar el paquete `Microsoft.Extensions.Options` a Infrastructure.

### 5. Correcciones de seguridad y consistencia

- **`CoreDataService`**: el bloque de validación de sesión (GetCoreUserAsync + 401) estaba duplicado en 4 métodos; se extrajo a `GetValidSessionAsync`.
- **Operaciones de escritura**: `CoreDataRepository` usaba `QueryAsync` para INSERT/UPDATE/DELETE; se reemplazó por `ExecuteAsync` (semántica correcta) y `QueryFirstAsync` para obtener el `Data_Id` del INSERT.
- **Fuga de datos del usuario**: `UpdateAsync`/`DeleteAsync` usan el `User_Id` de la sesión validada, no el del request.
- **Hasher**: constantes extraídas (salt 16, key 32, iteraciones 100k PBKDF2-HMACSHA256).

### 6. Connection strings: `SqlServer` (testing) y `SqlServerWeb` (producción)

**Decisión**: la connection string se resuelve por entorno en el composition root. En `Development` se usa `SqlServer`; en cualquier otro entorno se usa `SqlServerWeb`. Ambas son obligatorias: si la clave requerida no existe se lanza `InvalidOperationException` con mensaje claro.

**Motivación**: separar la BD de pruebas (`db_testing`) de la de producción. El valor de `SqlServerWeb` se tomó de la API original (`WebApi/appsettings.json`) y en `db_testing` ambas apuntan al mismo servidor por el momento.

### 7. Rate limiting (protección contra ataques)

**Decisión**: se usa el rate limiter integrado de ASP.NET Core (`AddRateLimiter`/`UseRateLimiter`, sin paquete adicional) con la política `client_25_per_minute` aplicada a `AuthController` y `CoreController`.

**Parámetros**: `FixedWindowLimiter` con `PermitLimit = 25` y `Window = 60s`, configurables en `appsettings.json` (`RateLimit:PermitLimit` / `RateLimit:WindowSeconds`). `QueueLimit = 0`: no hay cola, se rechaza de inmediato.

**Particionado por cliente**: la clave es el header `X-Forwarded-For` (primer valor) si viene — necesario en hosting compartido con reverse proxy, donde `RemoteIpAddress` es la IP del proxy — y cae a `RemoteIpAddress` si no viene.

**Respuesta**: los rechazos devuelven 429 en el mismo envelope `ApiResponse` (vía `OnRejected`), consistente con el resto de la API.

**Motivación**: limitar fuerza bruta/abuso sin afectar el uso normal (API de pocos usuarios). Verificado en runtime: pasan 25 requests y el 26.º devuelve 429.

### 8. CORS configurable y reversión de la caché de ApiKey

**CORS**: los orígenes permitidos se movieron a `appsettings.json` (`Cors:AllowedOrigins`). Se eliminó `SetIsOriginAllowed(_ => true)` (heredado del original) que **anulaba** la allow-list y permitía credenciales desde cualquier origen. Ahora la allow-list se aplica de verdad y se puede editar en producción sin recompilar. CORS solo aplica a navegadores; no afecta clientes nativos (MAUI, Postman, server-to-server).

**Caché de ApiKey**: se implementó un `IMemoryCache` en `ApiKeyFilter` y luego **se revirtió por decisión** (regla 14, no sobreingeniería): la API tiene pocos usuarios y no es masiva; validar la ApiKey contra BD por request no es un cuello de botella real. La caché agregaba estado en memoria y una ventana de 5 min ante rotación de clave sin beneficio concreto. Se mantiene la validación directa contra BD.

## Pendiente de revisión / deuda conocida

- **Tests de integración**: no usan el SP ni `Mae_Config` en el setup (INSERT directo en `Auth_Users`), por lo que no verifican `IsEnableRegister`; el SP se prueba implícitamente solo en `AuthRepositoryTests.CreateUserAsync_WithValidData_ReturnsSuccess`. Requieren `db_testing` con esquema (Auth_Users, Auth_Profiles, Mae_Config, PM_CoreData y SP Auth_Register).
- **`SqlServerWeb`**: apunta a `db_testing` como placeholder; confirmar la connection string real de producción antes de desplegar.
- **`JWT:Key`**: la clave del repo es pública; reemplazarla por un secreto único en el `appsettings.json` de producción.

## Próximos pasos

1. Antes de producción: confirmar `SqlServerWeb` real, reemplazar `JWT:Key`, ajustar `Cors:AllowedOrigins` y `RateLimit` en el `appsettings.json` de producción.