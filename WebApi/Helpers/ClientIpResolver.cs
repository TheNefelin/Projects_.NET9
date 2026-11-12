namespace WebApi.Helpers;

public static class ClientIpResolver
{
    // La IP del cliente la resuelve el runtime: el middleware ForwardedHeaders
    // reemplaza RemoteIpAddress por el X-Forwarded-For SOLO cuando el emisor es
    // un proxy conocido (ForwardedHeaders:KnownProxies/ForwardLimit). No se lee
    // el header aqui: leerlo directo permitiria a cualquier cliente forjarlo y
    // evadir rate limit, lockout y Retry-After.
    public static string Resolve(HttpContext httpContext)
    {
        return httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
    }
}
