using MyMauiApp.Models;
using MyMauiApp.Settings;
using System.Net.Http.Json;

namespace MyMauiApp.Services;

public class CoreService : ICoreService
{
    private readonly HttpClient _httpClient;
    private const string API_URL = $"{AppSettings.API_URL}/core";
    private const string API_KEY = AppSettings.API_KEY;

    public CoreService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    private async Task SetAuthHeadersAsync()
    {
        var jwtToken = await SecureStorage.GetAsync("ApiToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

        if (_httpClient.DefaultRequestHeaders.Contains("ApiKey"))
            _httpClient.DefaultRequestHeaders.Remove("ApiKey");

        _httpClient.DefaultRequestHeaders.Add("ApiKey", API_KEY);
    }

    public async Task<ApiResponse<CoreUserIV>> RegisterPasswordAsync(CorePasswordRequest corePasswordRequest)
    {
        try
        {
            await SetAuthHeadersAsync();
            var response = await _httpClient.PostAsJsonAsync($"{API_URL}/register-password", corePasswordRequest);

            if (!response.IsSuccessStatusCode)
                return new ApiResponse<CoreUserIV> { IsSuccess = false, Message = response.RequestMessage.ToString() };

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CoreUserIV>>();
            return result;
        }
        catch (Exception ex)
        {
            return new ApiResponse<CoreUserIV> { IsSuccess = false, Message = ex.Message };
        }
    }

    public async Task<ApiResponse<CoreUserIV>> GetCoreUserIVAsync(CorePasswordRequest corePasswordRequest)
    {
        try
        {
            await SetAuthHeadersAsync();
            var response = await _httpClient.PostAsJsonAsync($"{API_URL}/get-iv", corePasswordRequest);

            if (!response.IsSuccessStatusCode)
                return new ApiResponse<CoreUserIV> { IsSuccess = false, Message = response.RequestMessage.ToString() };

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CoreUserIV>>();
            return result;
        }
        catch (Exception ex)
        {
            return new ApiResponse<CoreUserIV> { IsSuccess = false, Message = ex.Message };
        }
    }

    public async Task<ApiResponse<IEnumerable<CoreUserData>>> GetAllCore(CoreUserRequest coreUserRequest)
    {
        try
        {
            await SetAuthHeadersAsync();
            var response = await _httpClient.GetAsync($"{API_URL}?User_Id={coreUserRequest.User_Id}&SqlToken={coreUserRequest.SqlToken}");

            if (!response.IsSuccessStatusCode)
                return new ApiResponse<IEnumerable<CoreUserData>> { IsSuccess = false, Message = response.RequestMessage.ToString() };

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<IEnumerable<CoreUserData>>>();
            return result;
        }
        catch (Exception ex)
        {
            return new ApiResponse<IEnumerable<CoreUserData>> { IsSuccess = false, Message = ex.Message };
        }
    }

    public async Task<ApiResponse<CoreUserData>> InsertCore(CoreDataRequest coreDataRequest)
    {
        try
        {
            await SetAuthHeadersAsync();
            var response = await _httpClient.PostAsJsonAsync($"{API_URL}", coreDataRequest);

            if (!response.IsSuccessStatusCode)
                return new ApiResponse<CoreUserData> { IsSuccess = false, Message = response.RequestMessage.ToString() };

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CoreUserData>>();
            return result;
        }
        catch (Exception ex)
        {
            return new ApiResponse<CoreUserData> { IsSuccess = false, Message = ex.Message };
        }
    }

    public async Task<ApiResponse<CoreUserData>> UpdateCore(CoreDataRequest coreDataRequest)
    {
        try
        {
            await SetAuthHeadersAsync();
            var response = await _httpClient.PutAsJsonAsync($"{API_URL}", coreDataRequest);

            if (!response.IsSuccessStatusCode)
                return new ApiResponse<CoreUserData> { IsSuccess = false, Message = response.RequestMessage.ToString() };

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<CoreUserData>>();
            return result;
        }
        catch (Exception ex)
        {
            return new ApiResponse<CoreUserData> { IsSuccess = false, Message = ex.Message };
        }
    }

    public async Task<ApiResponse<object>> DeleteCore(CoreDataDelete coreDataDelete)
    {
        try
        {
            await SetAuthHeadersAsync();
            var request = new HttpRequestMessage(HttpMethod.Delete, API_URL)
            {
                Content = JsonContent.Create(coreDataDelete)
            };
            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                return new ApiResponse<object> { IsSuccess = false, Message = response.RequestMessage.ToString() };

            var result = await response.Content.ReadFromJsonAsync<ApiResponse<object>>();
            return result;
        }
        catch (Exception ex)
        {
            return new ApiResponse<object> { IsSuccess = false, Message = ex.Message };
        }
    }
}
