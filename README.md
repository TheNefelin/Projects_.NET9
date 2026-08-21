# Projects .NET 9

# WebApi

API .NET 9 que agrupa las funcionalidades **CORE** (Password Manager) y **Auth** en una sola aplicación, implementada con **Clean Architecture** y **SOLID**.

[Free ASP Hosting](https://freeasphosting.net/)

## Stack

- .NET 9 (net9.0)
- SQL Server como única base de datos
- Dapper como ORM de acceso a datos
- JWT Bearer para autenticación
- ApiKey global por header con lockout por IP independiente
- Rate limiting integrado de ASP.NET Core (3 políticas: 25 req/min general, 5/min login, 5/min register)
- IP lockout con `TimeProvider` (login y ApiKey separados)
- Swagger (Swashbuckle 6.6.2) para documentación en la raíz

## Estructura de proyectos

```
Projects_.NET9/
│
├── WebApi/                     ← Host / Composition Root (API vigente)
│   ├── Controllers/
│   ├── Filters/
│   ├── Helpers/
│   ├── Health/
│   ├── Middleware/
│   ├── Program.cs
│   └── appsettings.json
│
├── WebApiPM.Domain/            ← Capa central, sin dependencias
│   ├── Entities/
│   ├── Interfaces/
│   └── Models/
│
├── WebApiPM.Application/       ← Casos de uso (reglas de negocio)
│   ├── Common/
│   ├── DTOs/
│   ├── Interfaces/
│   └── Services/
│
├── WebApiPM.Infrastructure/    ← Persistencia y servicios externos
│   ├── Data/
│   ├── Options/
│   ├── Repositories/
│   └── Security/
│
└── Utils/
```

### Grafo de referencias

Las dependencias apuntan siempre hacia adentro (Domain no depende de nadie):

```
WebApi ──> WebApiPM.Infrastructure ──> WebApiPM.Application ──> WebApiPM.Domain
```

## Dependencias por proyecto

### WebApi (host)

| Paquete | Versión | Propósito |
|---------|---------|-----------|
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 9.0.19 | Valida y autentica tokens JWT en cada petición (`[Authorize]`), con respuestas 401 estandarizadas. |
| `Microsoft.AspNetCore.OpenApi` | 9.0.19 | Genera el documento OpenAPI. |
| `Swashbuckle.AspNetCore` | 6.6.2 | UI interactiva de Swagger con botón de autenticación Bearer. |

### WebApiPM.Domain

Sin paquetes externos. Solo C# puro: entidades, interfaces de repositorio y modelos de resultado de BD.

### WebApiPM.Application

Sin paquetes externos. Solo referencia a `WebApiPM.Domain`. Contiene DTOs, interfaces de servicios y los casos de uso.

### WebApiPM.Infrastructure

| Paquete | Versión | Propósito |
|---------|---------|-----------|
| `Dapper` | 2.1.79 | Micro-ORM usado por los repositorios para ejecutar SQL/SPs contra SQL Server. |
| `Microsoft.Data.SqlClient` | 7.0.2 | Driver ADO.NET que permite a Dapper conectarse a SQL Server. |
| `System.IdentityModel.Tokens.Jwt` | 8.22.0 | Creación y firma de tokens JWT (`JwtTokenUtil`, implementa `IAuthTokenService`). |

## Decisiones técnicas

- **Envelope de respuesta único**: todas las respuestas (éxito y error) usan `ApiResponse<T>`. Validación, JWT 401, 404, excepciones y rate limit 429 se normalizan a ese formato; se descartó ProblemDetails por no romper la compatibilidad con los clientes actuales.
- **Sin fuga de excepciones**: los servicios no capturan excepciones; los errores inesperados llegan a un `IExceptionHandler` global que responde con `ApiResponse` 500 genérico.
- **Configuración JWT**: se vincula la sección `JWT` de `appsettings.json` al POCO `JwtOptions` (registrado como singleton en el host) con fail-fast al arrancar.
- **Connection strings por entorno**: `Development` usa `SqlServer` (testing); cualquier otro entorno usa `SqlServerWeb` (producción). Ambas obligatorias en config.
- **CORS configurable**: orígenes permitidos en `appsettings.json` (`Cors:AllowedOrigins`); la allow-list se aplica realmente. Solo afecta a navegadores; no a clientes nativos (MAUI) ni server-to-server.
- **Rate limiting**: tres políticas de ventana fija por IP (`client_25_per_minute`, `login_5_per_minute`, `register_5_per_minute`); rechazos en 429 `ApiResponse`. Parámetros en `appsettings.json`. No requiere paquete adicional.
- **IP lockout**: dos instancias de `IpLockoutService` (login y ApiKey) con ventana/duración distintas y `429` + `Retry-After`; testeable con `TimeProvider`.
- **Caché de ApiKey**: validación contra BD en tiempo constante, cacheada 30s (`ApiKeyCache:ExpirationSeconds`) para no golpear la BD en cada request.
- **Security headers**: aplicados solo a respuestas JSON (no bloquean Swagger).
- **Contraseñas**: PBKDF2-HMACSHA256 con salt 16 bytes, key 32 bytes y 100k iteraciones (`PasswordHasher`).

## Configuración

`appsettings.json` requiere:

```json
{
  "ConnectionStrings": {
    "SqlServer": "Server=localhost; Database=db_testing; User ID=testing; Password=testing; TrustServerCertificate=True;",
    "SqlServerWeb": "Data Source=localhost; Initial Catalog=db_testing; User ID=testing; Password=testing; TrustServerCertificate=True;"
  },
  "Cors": {
    "AllowedOrigins": ["https://www.francisco-dev.cl"]
  },
  "RateLimit": {
    "PermitLimit": 25,
    "WindowSeconds": 60,
    "LoginPermitLimit": 5,
    "LoginWindowSeconds": 60,
    "RegisterPermitLimit": 5,
    "RegisterWindowSeconds": 60
  },
  "ApiKeyCache": {
    "ExpirationSeconds": 30
  },
  "JWT": {
    "Key": "...",
    "Issuer": "...",
    "Audience": "...",
    "ExpireMin": 60
  }
}
```

> En producción (hosting sin variables de entorno) se reemplaza `appsettings.json` con los valores reales (`SqlServerWeb` apuntando a la BD de producción y una `JWT:Key` secreta). La selección es automática por entorno: `Development` → `SqlServer`, cualquier otro → `SqlServerWeb`.

## Estado

- Build: 0 errores / 0 warnings.
- Swagger: funcionando en la raíz.
- Tests: pendientes (por ejecutar contra `db_testing`).
- Auditoría: realizada; hay un hallazgo de seguridad pendiente (spoofing de IP en `ClientIpResolver`). Ver `DEVELOPMENT.md`.