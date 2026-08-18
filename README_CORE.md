# WebApiCore

API .NET 9 que agrupa las funcionalidades **CORE** (Password Manager) y **Auth** en una sola aplicación, implementada con **Clean Architecture** y **SOLID**. Sustituye a los proyectos `WebApi`, `ProjectAuth` y `ProjectPasswordManager` (que se conservan intactos durante la migración).

## Stack

- .NET 9 (net9.0)
- SQL Server como única base de datos
- Dapper como ORM de acceso a datos
- JWT Bearer para autenticación
- Rate limiting integrado de ASP.NET Core (25 req/min por cliente)
- Swagger (Swashbuckle) para documentación

## Estructura de proyectos

```
Projects_.NET9/
│
├── WebApiCore/                 ← Host / Composition Root (API)
│   ├── Controllers/
│   ├── Filters/
│   ├── Middleware/
│   ├── Program.cs
│   └── appsettings.json
│
├── WebApiCore.Domain/          ← Capa central, sin dependencias
│   ├── Entities/
│   ├── Interfaces/
│   └── Models/
│
├── WebApiCore.Application/     ← Casos de uso (reglas de negocio)
│   ├── Common/
│   ├── DTOs/
│   ├── Interfaces/
│   └── Services/
│
├── WebApiCore.Infrastructure/  ← Persistencia y servicios externos
│   ├── Data/
│   ├── Options/
│   ├── Repositories/
│   └── Security/
│
└── WebApiCore.Tests/           ← Tests de integración (xUnit)
```

### Grafo de referencias

Las dependencias apuntan siempre hacia adentro (Domain no depende de nadie):

```
WebApiCore ──┬──> WebApiCore.Application ──> WebApiCore.Domain
             └──> WebApiCore.Infrastructure ──> WebApiCore.Domain
                                     └──> WebApiCore.Application
WebApiCore.Tests ──> WebApiCore.Application
                 └──> WebApiCore.Infrastructure
```

## Dependencias por proyecto

### WebApiCore (host)

| Paquete | Versión | Propósito |
|---------|---------|-----------|
| `Microsoft.AspNetCore.Authentication.JwtBearer` | 9.0.19 | Valida y autentica tokens JWT en cada petición (`[Authorize]`), con respuestas 401 estandarizadas. |
| `Microsoft.AspNetCore.OpenApi` | 9.0.19 | Genera el documento OpenAPI del endpoint `/openapi/v1.json`. |
| `Swashbuckle.AspNetCore` | 9.0.6 | UI interactiva de Swagger con botón de autenticación Bearer. |

### WebApiCore.Domain

Sin paquetes externos. Solo C# puro: entidades, interfaces de repositorio y modelos de resultado de BD.

### WebApiCore.Application

Sin paquetes externos. Solo referencia a `WebApiCore.Domain`. Contiene DTOs, interfaces de servicios y los casos de uso.

### WebApiCore.Infrastructure

| Paquete | Versión | Propósito |
|---------|---------|-----------|
| `Dapper` | 2.1.79 | Micro-ORM usado por los repositorios para ejecutar SQL/SPs contra SQL Server. |
| `Microsoft.Data.SqlClient` | 7.0.2 | Driver ADO.NET que permite a Dapper conectarse a SQL Server. |
| `System.IdentityModel.Tokens.Jwt` | 8.22.0 | Creación y firma de tokens JWT (`JwtTokenUtil`, implementa `IAuthTokenService`). |
| `Microsoft.AspNetCore.Cryptography.KeyDerivation` | 9.0.19 | Derivación de claves PBKDF2-HMACSHA256 para el hasheo de contraseñas (`PasswordHasher`, implementa `IPasswordHasher`). |

### WebApiCore.Tests

| Paquete | Versión | Propósito |
|---------|---------|-----------|
| `xunit` | 2.9.2 | Framework de pruebas unitarias/integración. |
| `xunit.runner.visualstudio` | 2.8.2 | Adaptador para ejecutar tests desde Visual Studio / `dotnet test`. |
| `Microsoft.NET.Test.Sdk` | 17.12.0 | Host de ejecución de tests en .NET. |
| `coverlet.collector` | 6.0.2 | Recolector de cobertura de código. |

## Decisiones técnicas

- **Envelope de respuesta único**: todas las respuestas (éxito y error) usan `ApiResponse<T>`. Validación, JWT 401, 404, excepciones y rate limit 429 se normalizan a ese formato; se descartó ProblemDetails por no romper la compatibilidad con los clientes actuales.
- **Sin fuga de excepciones**: los servicios no capturan excepciones; los errores inesperados llegan a un `IExceptionHandler` global que responde con `ApiResponse` 500 genérico.
- **Configuración JWT**: se vincula la sección `JWT` de `appsettings.json` al POCO `JwtOptions` (registrado como singleton en el host).
- **Connection strings por entorno**: `Development` usa `SqlServer` (testing); cualquier otro entorno usa `SqlServerWeb` (producción). Ambas obligatorias en config.
- **CORS configurable**: orígenes permitidos en `appsettings.json` (`Cors:AllowedOrigins`); la allow-list se aplica realmente (se eliminó `SetIsOriginAllowed(_ => true)` que la anulaba). Solo afecta a navegadores; no a clientes nativos (MAUI) ni server-to-server.
- **Rate limiting**: política `client_25_per_minute` (25 req/min por cliente, ventana fija de 60s) aplicada a los controllers; particionado por `X-Forwarded-For`/IP; rechazos en 429 `ApiResponse`. Parámetros en `appsettings.json` (`RateLimit:PermitLimit`/`RateLimit:WindowSeconds`). No requiere paquete adicional (rate limiter del shared framework).

## Configuración

`appsettings.json` requiere:

```json
{
  "ConnectionStrings": {
    "SqlServer": "Server=localhost; Database=db_testing; User ID=testing; Password=testing; TrustServerCertificate=True;",
    "SqlServerWeb": "Data Source=localhost; Initial Catalog=db_testing; User ID=testing; Password=testing; TrustServerCertificate=True;"
  },
  "Cors": {
    "AllowedOrigins": ["http://localhost:4200", "http://localhost:3000"]
  },
  "RateLimit": {
    "PermitLimit": 25,
    "WindowSeconds": 60
  },
  "JWT": {
    "Key": "...",
    "Issuer": "...",
    "Audience": "...",
    "Subject": "...",
    "ExpireMin": 60
  }
}
```

> En producción (hosting sin variables de entorno) se reemplaza `appsettings.json` con los valores reales (`SqlServerWeb` apuntando a la BD de producción y una `JWT:Key` secreta). La selección es automática por entorno: `Development` → `SqlServer`, cualquier otro → `SqlServerWeb`.