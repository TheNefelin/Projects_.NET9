using MyMauiApp.Models;
using MyMauiApp.Settings;
using System.Net.Http.Json;

namespace MyMauiApp.Services;

public class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;
    private const string API_URL = $"{AppSettings.API_URL}/auth";
    private const string API_KEY = AppSettings.API_KEY;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("ApiKey", API_KEY);
    }

    public async Task<ApiResponse<AuthUserLogged>> LoginAsync(AuthUserLogin credentials)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{API_URL}/login", credentials);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AuthUserLogged>>();

            if (result?.IsSuccess == true && result.Data != null)
            {
                await SecureStorage.SetAsync("UserId", result.Data.User_Id.ToString());
                await SecureStorage.SetAsync("SqlToken", result.Data.SqlToken.ToString());
                await SecureStorage.SetAsync("Role", result.Data.Role.ToString());
                await SecureStorage.SetAsync("ExpireMin", result.Data.ExpireMin);
                await SecureStorage.SetAsync("ApiToken", result.Data.ApiToken);
            }

            return result ?? new ApiResponse<AuthUserLogged> { IsSuccess = false, Message = "Error" };
        }
        catch (Exception ex)
        {
            return new ApiResponse<AuthUserLogged> { IsSuccess = false, Message = ex.Message };
        }
    }

    public async Task LogoutAsync()
    {
        SecureStorage.Remove("UserId");
        SecureStorage.Remove("SqlToken");
        SecureStorage.Remove("Role");
        SecureStorage.Remove("ExpireMin");
        SecureStorage.Remove("ApiToken");
        await Task.CompletedTask;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var token = await GetJwtTokenAsync();
        return !string.IsNullOrEmpty(token);
    }

    public async Task<string?> GetJwtTokenAsync()
    {
        return await SecureStorage.GetAsync("ApiToken");
    }

    public async Task<string?> GetSqlTokenAsync()
    {
        return await SecureStorage.GetAsync("SqlToken");
    }

    public async Task<int> GetExpireMinAsync()
    {
        string expireMinString =  await SecureStorage.GetAsync("ExpireMin");

        if (string.IsNullOrEmpty(expireMinString))
        {
            return 0;
        }

        return int.Parse(expireMinString);
    }

    public async Task<string?> GetUserIdAsync()
    {
        return await SecureStorage.GetAsync("UserId");
    }

    public async Task<string?> GetRoleAsync()
    {
        return await SecureStorage.GetAsync("Role");
    }
}
