using WebApiPM.Infrastructure.Options;

namespace WebApiPM.Tests.Helpers;

public static class TestJwtOptions
{
    public static JwtOptions Create() => new()
    {
        Key = "TestKeyTestKeyTestKeyTestKeyTestKeyTestKey",
        Issuer = "TestIssuer",
        Audience = "TestAudience",
        ExpireMin = 60
    };
}