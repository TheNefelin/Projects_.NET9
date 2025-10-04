namespace MyMauiApp.Services;

public interface ISecureStorageService
{
    Task SetSessionAsync(string userId, string sqlToken, string role, string expireMin, string apiToken, DateTime expirationTime);
    Task ClearSessionAsync();
    Task<string?> GetUserIdAsync();
    Task<string?> GetSqlTokenAsync();
    Task<string?> GetRoleAsync();
    Task<string?> GetApiTokenAsync();
    Task<int> GetExpireMinAsync();
    Task<DateTime?> GetExpirationTimeAsync();
    Task<bool> IsAuthenticatedAsync();
}
