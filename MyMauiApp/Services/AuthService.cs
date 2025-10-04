using MyMauiApp.Models;
using MyMauiApp.Settings;
using System.Net.Http.Json;

namespace MyMauiApp.Services;

public class AuthService : IAuthService
{
    private readonly ISecureStorageService _storageService;
    private readonly HttpClient _httpClient;
    private const string API_URL = $"{AppSettings.API_URL}/auth";
    private const string API_KEY = AppSettings.API_KEY;

    public AuthService(HttpClient httpClient, ISecureStorageService storageService)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Add("ApiKey", API_KEY);
        _storageService = storageService;
    }

    public async Task<ApiResponse<AuthUserLogged>> LoginAsync(AuthUserLogin credentials)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync($"{API_URL}/login", credentials);
            var result = await response.Content.ReadFromJsonAsync<ApiResponse<AuthUserLogged>>();

            if (result?.IsSuccess == true && result.Data != null)
            {
                var expirationTime = DateTime.Now.AddMinutes(int.Parse(result.Data.ExpireMin));

                await _storageService.SetSessionAsync(
                      result.Data.User_Id.ToString(),
                      result.Data.SqlToken.ToString(),
                      result.Data.Role,
                      result.Data.ExpireMin,
                      result.Data.ApiToken,
                      expirationTime
                  );
            }

            return result ?? new ApiResponse<AuthUserLogged> { IsSuccess = false, Message = "Error" };
        }
        catch (Exception ex)
        {
            return new ApiResponse<AuthUserLogged> { IsSuccess = false, Message = ex.Message };
        }
    }

    public async Task LogoutAsync() => await _storageService.ClearSessionAsync();
}
