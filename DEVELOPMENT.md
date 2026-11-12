# DEVELOPMENT - WebApi (API consolidada)

Documento de decisiones y estado de la API consolidada `.NET 9` con Clean Architecture y SOLID.

## Estado

- **Vigente**: `WebApi` (host/composition root) + `WebApiPM.Domain` + `WebApiPM.Application` + `WebApiPM.Infrastructure`. Es la API actual, más SENIOR y robusta que las versiones anteriores.
- **Build**: 0 errores / 0 warnings.
- **Swagger**: funcionando en la raíz (`RoutePrefix = string.Empty`) con Swashbuckle 6.6.2 / OpenApi 1.x. El mapa de la API **no requiere ApiKey**, así que queda **expuesto por decisión explícita** mediante la clave `OpenApi:Enabled` (por defecto `true`, también en producción): el documento sirve de reconocimiento (endpoints, headers y esquemas), no de acceso. El interruptor está en configuración y no depende de `ASPNETCORE_ENVIRONMENT` a propósito, para que un entorno mal puesto no decida por accidente si el documento queda público. Para desactivarlo en un entorno concreto: `"OpenApi": { "Enabled": false }`. Mismo criterio y misma clave que `WebApiCore` (.NET 10).
- **Tests**: **72/72 en verde** contra la BD local `db_testing` (runner MTP; `dotnet WebApiPM.Tests\bin\Debug\net9.0\WebApiPM.Tests.dll`). Incluye los 3 de health checks (`/health/live`, `/health/ready` y el alias `/health`), los de contrato `ProblemDetails` y los de paridad con `WebApiCore`.
- **Esquema**: idéntico al de la versión .NET 10 (`PasswordManager_.NET10`), así que **ambas APIs pueden compartir la misma base de datos**.
- **Utils**: existe como proyecto pero no usado actualmente; reservado para componentes compartidos futuros.
- **Auditoría de código**: realizada. El hallazgo crítico (spoofing de IP) está **corregido**; los menores pendientes están en [Auditoría](#auditoría-hallazgos-pendientes).

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

### 1. Contrato de respuesta: DTO plano + `ProblemDetails` (RFC 9457)

**Decisión**: los servicios devuelven el **DTO directamente** (sin envelope) y todos los errores salen como **`application/problem+json`** con `status`/`title`/`detail`/`traceId`. Los caminos de error son cinco y todos unifican el mismo formato:
- Validación de modelo → `InvalidModelStateResponseFactory` devuelve `ValidationProblemDetails` (400).
- JWT 401 → `JwtBearerEvents.OnChallenge` (401).
- Rate limiter → `OnRejected` (429).
- 404 de rutas inexistentes → `MapFallback` (404).
- Excepciones no controladas → `GlobalExceptionHandler` (`IExceptionHandler`): 400 validación, 403 registro deshabilitado, 409 email duplicado, 429 demasiados intentos, 401 credenciales/sesión inválida, 404 recurso inexistente, 500 genérico.

**Motivación**: `ApiResponse<T>` duplicaba información que el status HTTP ya transportaba y obligaba al cliente a desenvolver cada respuesta. Con `ProblemDetails` el cliente deserializa un único tipo de error.

**Consecuencia práctica**: **el cliente MAUI v10 (`PasswordManager_.NET10`) habla con esta API y con `WebApiCore` (.NET 10) indistintamente**, porque ambas exponen exactamente el mismo contrato (mismas rutas, mismos headers, mismos DTOs y mismo formato de error). Es el motivo de esta decisión.

> ℹ️ Historia: esta API mantenía antes el envelope `ApiResponse<T>` (decisión documentada, con sus clientes actuales). La migración al formato plano fue **deliberadamente breaking** y se hizo junto con el cliente, precisamente para habilitar el uso de un único cliente (MAUI v10) contra las dos APIs.

### 1 bis. Sin stored procedures: Dapper con SQL parametrizado

**Decisión**: el alta de usuario es un `INSERT` parametrizado en `AuthUserRepository` (con `Profile_Id = 2`) y el login se resuelve en C# (`GetUserByEmailAsync` + verificación del hash + token nuevo). El repositorio devuelve un enum `UserCreationStatus` (`Created`, `EmailAlreadyExists`) en vez del contrato `SqlResponse` del SP.

**Motivación**: la lógica de negocio queda en código testeable (sin round-trip extra a la BD) y el esquema deja de depender de procedimientos almacenados. El flag `Mae_Config.IsEnableRegister`, que validaba el SP `Auth_Register`, se comprobó ahora en el servicio (`AuthUserService.RegisterAsync` → 403).

**Consecuencia**: el script `SqlServerPM.sql` ya no crea los SP `Auth_Register`/`Auth_Login`.

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

### 8. Sin caché de ApiKey: validación en cada request

**Decisión**: `MaeConfigService.ValidateApiKey` consulta `Mae_Config` en **cada request** (antes cacheaba la ApiKey 30 s en memoria). La comparación sigue siendo en tiempo constante (`CryptographicOperations.FixedTimeEquals`).

**Motivación**: con la caché, una ApiKey filtrada seguía siendo válida durante la ventana de expiración; sin ella, la rotación surte efecto en el siguiente request. El costo es despreciable (una fila por PK) y el rate limit acota el tráfico. La clave `ApiKeyCache:ExpirationSeconds` se eliminó de `appsettings.json`.

### 8 bis. Token de sesión hasheado en BD

**Decisión**: `Auth_Users.SqlTokenHash VARCHAR(64)` guarda **solo el SHA-256** (hexadecimal minúsculas) del token de sesión; el token crudo se genera en C# al iniciar sesión y nunca se persiste. La comparación se hace por hash (`SqlTokenHasher.Hash`), y el `SqlToken` de la sesión se valida contra el hash almacenado.

**Motivación**: con el token en texto plano, quien leía la base obtenía una sesión reutilizable.

**Migración**: la base de **testing local** ya está en el esquema nuevo (`SqlTokenHash`, sin `SqlToken`); el script que hizo esa transición **no vive en este repositorio** (los scripts de migración y validación de producción quedan fuera del alcance: ver `AGENTS.md` §7 y §11). Para el desarrollo y las pruebas locales solo hace falta el esquema que define `SqlServerPM.sql`.

> ℹ️ Dato útil para quien tenga que aplicar el cambio en otra base: el hash debe calcularse sobre el **GUID canónico en minúsculas** (`Guid.ToString("D")`), porque SQL Server renderiza el `UNIQUEIDENTIFIER` en mayúsculas y .NET hashea los bytes UTF-8 de la forma en minúsculas. Con GUIDs que contienen letras, hashear sin normalizar a minúsculas produce un hash distinto y las sesiones existentes quedan invalidadas.

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
- **Hasher**: PBKDF2-HMACSHA256 con salt 16 bytes, key 32 bytes y **600 000 iteraciones** (`PasswordHasher`), con **fallback a 100 000** para verificar los hashes ya persistidos (los legacy se re-derivan al re-registrar o cambiar la clave maestra).
- **CORS**: orígenes permitidos en `appsettings.json` (`Cors:AllowedOrigins`); la allow-list se aplica realmente (se eliminó `SetIsOriginAllowed(_ => true)` que la anulaba). Solo afecta a navegadores; no a clientes nativos (MAUI) ni server-to-server.
- **Token de sesión fuera de la query string**: el `GET /api/core` toma el `SqlToken` del **header `SqlToken`** (antes viajaba en la URL, donde quedaba expuesto en logs, proxies e historial). El `User_Id` del cuerpo se ignora: la identidad sale del claim `sub` del JWT.
- **Header de credencial `X-ApiKey`**: el header global se renombró de `ApiKey` a `X-ApiKey` (filtro de validación, documento de OpenAPI y tests). Los nombres de header no distinguen mayúsculas y el prefijo `X-` está en desuso por RFC 6648: es **convención, no seguridad**.
- **Health checks con liveness separado de readiness**: `/health/live` (predicado `false`, no toca la BD), `/health/ready` (predicado `true`, incluye `SqlHealthCheck` → `SELECT 1` vía Dapper) y `/health` como **alias de readiness** para no romper el contrato previo. Los tres son `AllowAnonymous`.
- **Lockout por IP de la clave maestra**: `get-iv` y `change-password` cuentan los fallos y bloquean la IP (contador compartido con el de login): 5 fallos → bloqueo. Un login fallido no debe permitir probar la clave maestra indefinidamente.
- **Cambio de clave maestra (`POST /api/core/change-password`)**: rotación **atómica** de hash, sal y token de sesión + reemplazo de los registros re-cifrados por el cliente, todo en **una sola transacción** (el contrato que evita perder datos si el request falla a mitad). La nueva sal/IV (16 bytes, base64) la genera el cliente, que es el dueño del cifrado; el servidor solo valida su formato.
- **Política de longitud de la clave maestra**: **6** para verificar (`CoreUserPassword`, `ChangeCorePasswordRequest.OldPassword`) y **8** para crear o cambiar (`CoreUserPasswordCreate`, `ChangeCorePasswordRequest.NewPassword`). Sin reglas de composición: lo que protege es longitud + lockout + rate limit.
- **Central Package Management (CPM)**: `Directory.Packages.props` en la raíz con `ManagePackageVersionsCentrally=true`; las versiones de los `PackageReference` viven una sola vez ahí y los `.csproj` quedan sin `Version`.
- **Runner MTP**: `global.json` con `"test": { "runner": "Microsoft.Testing.Platform" }` y `UseMicrosoftTestingPlatformRunner` en el proyecto de tests (con el SDK de .NET 10, `dotnet test` con el target de VSTest ya no está soportado).
- **`Microsoft.AspNetCore.Cryptography.KeyDerivation` eliminado**: el PBKDF2 se usa desde el BCL (`Rfc2898DeriveBytes.Pbkdf2`); el paquete externo solo envolvía esa llamada.

## Auditoría (hallazgos pendientes)

### Crítico (corregido)

- ✅ **Spoofing de IP en `ClientIpResolver`**: corregido. `Program.cs` configura `ForwardedHeadersOptions` desde la sección `ForwardedHeaders` (`KnownProxies` + `ForwardLimit`) y `ClientIpResolver` usa **solo `Connection.RemoteIpAddress`**: el runtime reemplaza la IP desde `X-Forwarded-For` únicamente cuando el emisor es un proxy conocido, así que un cliente directo no puede forjar la IP que alimenta rate limit, lockout y `Retry-After`. La IP real se toma del header **solo** en los proxies declarados en `appsettings.json`.

### Menor

- **`NewSqlToken`**: valida el resultado del `UPDATE` (antes devolvía `Guid.Empty` si no afectaba filas, y el login "exitoso" entregaba un token inválido en silencio).
- **`ApiResponse.Success<object>(null!, ...)`** en `CoreDataService.DeleteAsync`: el envelope serializa `Data: null`; el cliente debe tolerarlo.
- **Logging de PII**: se registra el email completo en login/register. Aceptable hoy; revisar si se incorpora un sink de almacenamiento (Serilog) para no acumular PII sin anonimizar.
- **`SqlResponse`** (modelo del contrato del SP `Auth_Register`): quedó **sin uso** al pasar al INSERT parametrizado. Pendiente de eliminar.
- **`Utils`**: proyecto sin uso (contiene un `EncryptionService`/`PasswordUtil` duplicados). Pendiente de eliminar o de convertir en biblioteca compartida.
- ⚠️ **Claves maestras legacy**: la política de longitud (6) bloquea las claves de menos de 6 caracteres que se pudieran haber creado antes de la validación. No hay salida desde la app (el cambio de clave exige la clave actual). Riesgo asumido, con la misma advertencia que en la versión .NET 10.

## Próximos pasos

1. **CI**: no hay workflow en este repo (`.github/workflows` está vacío). Portar el de la versión .NET 10 (compila ambas suites y corre los tests contra un SQL Server efímero).
2. **Config de producción real**: `SqlServerWeb`, `JWT:Key` secreta, `Cors:AllowedOrigins` y `RateLimit` en el `appsettings.json` de producción. Lo gestiona el usuario (fuera del alcance del agente: ver `AGENTS.md` §7).
3. Eliminar `SqlResponse` y el proyecto `Utils` (sin uso).
4. **Verificar en el cliente MAUI v10** contra esta API en el entorno local (el contrato ya es idéntico al de `WebApiCore`): login, alta de clave maestra, CRUD de secretos y cambio de clave maestra.