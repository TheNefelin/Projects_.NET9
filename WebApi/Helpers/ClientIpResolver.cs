namespace WebApi.Helpers;

public static class ClientIpResolver
{
    public static string Resolve(HttpContext httpContext)
        => httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
}