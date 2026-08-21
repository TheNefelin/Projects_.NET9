# DEVELOPMENT - WebApi (API consolidada)

Documento de decisiones y estado de la API consolidada `.NET 9` con Clean Architecture y SOLID.

## Estado

- **Vigente**: `WebApi` (host/composition root) + `WebApiPM.Domain` + `WebApiPM.Application` + `WebApiPM.Infrastructure`. Es la API actual, más SENIOR y robusta que las versiones anteriores.
- **Build**: 0 errores / 0 warnings.
- **Swagger**: funcionando en la raíz (`RoutePrefix = string.Empty`) con Swashbuckle 6.6.2 / OpenApi 1.x.
- **Tests**: pendientes (por ejecutar contra `db_testing`).
- **Auditoría de código**: realizada. Un hallazgo de seguridad pendiente (spoofing de IP) y varios menores, documentados en [Auditoría](#auditoría-hallazgos-pendientes).

## Arquitectura

### Grafo de referencias

Las dependencias apuntan hacia adentro (Domain no depende de nada):

```
WebApi ──> WebApiPM.Infrastructure ──> WebApiPM.Application ──> WebApiPM.Domain
```

- **WebApiPM.Domain**: entidades, interfaces de repositorio y modelos (sin dependencias).
- **WebApiPM.Application**: DTOs, `ApiResponse<T>`, interfaces de servicios y casos de uso.
- **WebApiPM.Infrastructure**: `DapperContext`, repositorios, `PasswordHasher`, `JwtTokenUtil`, `IpLockoutService`, `JwtOptions`, `IpLockoutOptions`.
- **WebApi**: composition root (DI), controllers, filters, middleware, health checks.

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

**Decisión**: los casos de uso no capturan excepciones; los errores inesperados se propagan al `IExceptionHandler`, que responde con mensajes genéricos y registra la excepción real en logs.

**Motivación**: evita la fuga de detalles internos hacia el cliente y garantiza que el `GlobalExceptionHandler` sea quien controle el error 500.

### 3. DIP: dependencias por interfaz

**Decisión**: los casos de uso dependen de interfaces (`IPasswordHasher`, `IAuthTokenService`, `IIpLockoutService`, repositorios) definidas en Application/Domain e implementadas en Infrastructure.

**Motivación**: intercambiabilidad sin tocar Application.

### 4. Configuración JWT con Options pattern

**Decisión**: la sección `JWT` de `appsettings.json` se vincula al POCO `JwtOptions` (Infrastructure) y se registra como singleton en el host. Si la sección no existe, falla el arranque con mensaje claro (fail-fast).

**Motivación**: configuración centralizada, tipada y validada en tiempo de arranque.

### 5. Connection strings: `SqlServer` (testing) y `SqlServerWeb` (producción)

**Decisión**: la connection string se resuelve por entorno en el composition root. En `Development` se usa `SqlServer`; en cualquier otro entorno se usa `SqlServerWeb`. Ambas son obligatorias: si la clave requerida no existe se lanza `InvalidOperationException`.

### 6. Rate limiting (protección contra ataques)

**Decisión**: se usa el rate limiter integrado de ASP.NET Core (`AddRateLimiter`/`UseRateLimiter`, sin paquete adicional) con tres políticas de ventana fija, todas configurables en `appsettings.json`:

| Política | Aplicada a | Default |
|----------|-----------|---------|
| `client_25_per_minute` | `AuthController`, `CoreController` (nivel de clase) | 25 req / 60s |
| `login_5_per_minute` | `AuthController.Login` (sobreescribe la de clase) | 5 req / 60s |
| `register_5_per_minute` | `AuthController.Register` (sobreescribe la de clase) | 5 req / 60s |

- `QueueLimit = 0`: no hay cola, se rechaza de inmediato.
- Particionado por cliente (`ClientIpResolver`): header `X-Forwarded-For` (primer valor) si viene, con fallback a `RemoteIpAddress`.
- Los rechazos devuelven 429 en el mismo envelope `ApiResponse` (vía `OnRejected`), consistente con el resto de la API.

### 7. IP lockout separado para login y ApiKey

**Decisión**: se implementa `IpLockoutService` (en memoria, `ConcurrentDictionary` + `TimeProvider`) con dos instancias independientes:
- **Login/Register**: `MaxFailures = 5`, ventana 15 min, bloqueo 15 min.
- **ApiKey**: servicio keyed (`"api-key"`), `MaxFailures = 5`, ventana 10 min, bloqueo 1 hora.

Los fallos incrementan el contador; el éxito resetea; `GetRemainingBlockTime` permite responder `Retry-After` en el 429 del filter de ApiKey.

**Motivación**: dos políticas distintas porque la ApiKey es un secreto compartido estático (bloqueo más severo) y las credenciales de usuario son por-cuenta (bloqueo moderado).

### 8. Caché de ApiKey con TTL configurable

**Decisión**: `MaeConfigService` cachea la ApiKey de `Mae_Config` en memoria con TTL de 30s (`ApiKeyCache:ExpirationSeconds`), sincronizado con `Lock` y usando `TimeProvider`. Comparación de valores con `CryptographicOperations.FixedTimeEquals` (tiempo constante).

**Motivación**: reduce el round-trip a BD en cada request sin dejar una ventana larga ante rotación de la clave (30s).

### 9. Security headers solo en respuestas JSON

**Decisión**: `SecurityHeadersMiddleware` aplica `X-Content-Type-Options`, `X-Frame-Options`, `Referrer-Policy` y `Content-Security-Policy` únicamente cuando la respuesta es JSON (`ContentType` contiene `json`), vía `Response.OnStarting`.

**Motivación**: protege los endpoints de la API sin bloquear la UI y los assets de Swagger. La versión previa (excluir por path) era frágil y rompió el renderizado de Swagger en la raíz.

### 10. Swagger con Swashbuckle y OpenApi 1.x

**Decisión**: se usa **solo Swashbuckle** (`AddSwaggerGen`/`UseSwagger`/`UseSwaggerUI`) con `RoutePrefix = string.Empty` (UI en la raíz) y `SwaggerEndpoint("/swagger/v1/swagger.json")`. No se usa el `AddOpenApi()` nativo de .NET 9 (servía `/openapi/v1.json` y mezclaba dos documentos).

**Motivación**: un único documento Swagger. Los operation filters (`AuthorizeOperationFilter`, `ApiKeyOperationFilter`) usan la sintaxis de OpenApi 1.x (`.Type = "string"`, `OpenApiReference`), no la 2.x (`JsonSchemaType`, `IOpenApiParameter`), que no existe en Swashbuckle 6.6.2.

### 11. Correcciones de seguridad y consistencia

- **`CoreDataService`**: el bloque de validación de sesión (GetCoreUserAsync + 401) se extrajo a `GetValidSessionAsync`.
- **Operaciones de escritura**: `CoreDataRepository` usa `ExecuteAsync` para INSERT/UPDATE/DELETE (semántica correcta) y `QueryFirstAsync` para obtener el `Data_Id` del INSERT.
- **Fuga de datos del usuario**: `UpdateAsync`/`DeleteAsync` usan el `User_Id` de la sesión validada (claim JWT), no el del request.
- **Hasher**: PBKDF2-HMACSHA256 con salt 16 bytes, key 32 bytes y 100k iteraciones (`PasswordHasher`).
- **CORS**: orígenes permitidos en `appsettings.json` (`Cors:AllowedOrigins`); la allow-list se aplica realmente (se eliminó `SetIsOriginAllowed(_ => true)` que la anulaba). Solo afecta a navegadores; no a clientes nativos (MAUI) ni server-to-server.

## Auditoría (hallazgos pendientes)

### Crítico (pendiente de corrección)

- **Spoofing de IP en `ClientIpResolver`** (`WebApi/Helpers/ClientIpResolver.cs`): confía en el header `X-Forwarded-For` sin `UseForwardedHeaders` ni `KnownProxies`/`KnownNetworks`. Un atacante puede evadir el rate limiting y el IP lockout inventando un XFF distinto por request, o provocar un bloqueo de IP ajena (DoS dirigido).
  - **Fix propuesto (no aplicado)**: `app.UseForwardedHeaders` con `ForwardedHeaders.XForwardedFor | XForwardedProto` y `KnownProxies` configurables (loopback en dev, proxy real en producción); simplificar `ClientIpResolver` para usar `Connection.RemoteIpAddress` (el middleware lo actualiza solo con IPs confiables). Pendiente de autorización.

### Menores

- **`NewSqlToken`** (`AuthUserRepository.cs`): si el `UPDATE ... OUTPUT` no afecta filas, devuelve `Guid.Empty` y el login "exitoso" entrega un `SqlToken` inválido silenciosamente. Conviene validar el resultado.
- **`MaeConfigService.GetCachedApiKeyAsync`**: al expirar el TTL pueden darse varias consultas simultáneas a BD (thundering herd). Impacto bajo (1 fila), pero un `Lazy<T>` resolvería la semántica exacta.
- **`ApiResponse.Success<object>(null!, ...)`** en `CoreDataService.DeleteAsync`: el envelope serializa `Data: null`; el cliente debe tolerarlo.
- **Logging de PII**: se registra el email completo en login/register. Aceptable hoy; revisar si se incorpora un sink de almacenamiento (Serilog) para no acumular PII sin anonimizar.
- **SPs no verificables desde el repo**: `Auth_Register` y el mapeo a `SqlResponse` dependen de la BD (`SqlServer.sql`). Verificar el contrato del SP antes de desplegar.

## Próximos pasos

1. Aplicar el fix de spoofing de IP (`UseForwardedHeaders` + `ClientIpResolver`), pendiente de autorización.
2. Tests de integración contra `db_testing` (esquema: Auth_Users, Auth_Profiles, Mae_Config, PM_CoreData y SP Auth_Register).
3. Config de producción real: `SqlServerWeb`, `JWT:Key` secreta, `Cors:AllowedOrigins` y `RateLimit` en el `appsettings.json` de producción.