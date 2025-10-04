
namespace MyMauiApp.Services;

public class SecureStorageService : ISecureStorageService
{
    private const string KEY_USER_ID = "UserId";
    private const string KEY_SQL_TOKEN = "SqlToken";
    private const string KEY_ROLE = "Role";
    private const string KEY_EXPIRE_MIN = "ExpireMin";
    private const string KEY_API_TOKEN = "ApiToken";
    private const string KEY_EXPIRATION_TIME = "ExpirationTime";

    public async Task SetSessionAsync(string userId, string sqlToken, string role, string expireMin, string apiToken, DateTime expirationTime)
    {
        await SecureStorage.SetAsync(KEY_USER_ID, userId);
        await SecureStorage.SetAsync(KEY_SQL_TOKEN, sqlToken);
        await SecureStorage.SetAsync(KEY_ROLE, role);
        await SecureStorage.SetAsync(KEY_EXPIRE_MIN, expireMin);
        await SecureStorage.SetAsync(KEY_API_TOKEN, apiToken);
        await SecureStorage.SetAsync(KEY_EXPIRATION_TIME, expirationTime.ToString("o"));
    }

    public async Task ClearSessionAsync()
    {
        SecureStorage.Remove(KEY_USER_ID);
        SecureStorage.Remove(KEY_SQL_TOKEN);
        SecureStorage.Remove(KEY_ROLE);
        SecureStorage.Remove(KEY_EXPIRE_MIN);
        SecureStorage.Remove(KEY_API_TOKEN);
        SecureStorage.Remove(KEY_EXPIRATION_TIME);
        await Task.CompletedTask;
    }

    public async Task<string?> GetUserIdAsync() => await SecureStorage.GetAsync(KEY_USER_ID);

    public async Task<string?> GetSqlTokenAsync() => await SecureStorage.GetAsync(KEY_SQL_TOKEN);

    public async Task<string?> GetRoleAsync() => await SecureStorage.GetAsync(KEY_ROLE);

    public async Task<string?> GetApiTokenAsync() => await SecureStorage.GetAsync(KEY_API_TOKEN);

    public async Task<int> GetExpireMinAsync()
    {
        var expireMinString = await SecureStorage.GetAsync(KEY_EXPIRE_MIN);
        return string.IsNullOrEmpty(expireMinString) ? 0 : int.Parse(expireMinString);
    }

    public async Task<DateTime?> GetExpirationTimeAsync()
    {
        var expirationStr = await SecureStorage.GetAsync(KEY_EXPIRATION_TIME);
        if (string.IsNullOrEmpty(expirationStr)) return null;
        return DateTime.Parse(expirationStr);
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetApiTokenAsync();
        return !string.IsNullOrEmpty(token);
    }
}
