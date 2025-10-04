using MyMauiApp.Extensions;
using MyMauiApp.Models;
using MyMauiApp.Settings;
using System.Net.Http.Json;

namespace MyMauiApp.Services;

public class CoreService : ICoreService
{
    private readonly ISecureStorageService _secureStorageService;
    private readonly HttpClient _httpClient;
    private const string API_URL = $"{AppSettings.API_URL}/core";
    private const string API_KEY = AppSettings.API_KEY;

    public CoreService(HttpClient httpClient, ISecureStorageService secureStorageService)
    {
        _httpClient = httpClient;
        _secureStorageService = secureStorageService;
    }

    private async Task SetAuthHeadersAsync()
    {
        var jwtToken = await SecureStorage.GetAsync("ApiToken");

        _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", jwtToken);

        if (_httpClient.DefaultRequestHeaders.Contains("ApiKey"))
            _httpClient.DefaultRequestHeaders.Remove("ApiKey");

        _httpClient.DefaultRequestHeaders.Add("ApiKey", API_KEY);
    }

    public async Task<ApiResponse<CoreUserIV>> RegisterPasswordAsync(string password)
    {
        try
        {
            CoreUserRequest coreUserRequest = await GetCoreUser();
            var corePasswordRequest = new CorePasswordRequest
            {
                Password = password,
                CoreUser = coreUserRequest,
            };

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

    public async Task<ApiResponse<CoreUserIV>> GetCoreUserIVAsync(string password)
    {
        try
        {
            CoreUserRequest coreUserRequest = await GetCoreUser();
            var corePasswordRequest = new CorePasswordRequest
            {
                Password = password,
                CoreUser = coreUserRequest,
            };

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

    public async Task<ApiResponse<IEnumerable<CoreUserData>>> GetAllCore()
    {
        try
        {
            CoreUserRequest coreUserRequest = await GetCoreUser();

            await SetAuthHeadersAsync();
            var response = await _httpClient.GetAsync($"{API_URL}?User_Id={coreUserRequest.User_Id}&SqlToken={coreUserRequest.SqlToken}");

            if (await response.HandleUnauthorizedAsync())
                return new ApiResponse<IEnumerable<CoreUserData>> { IsSuccess = false, Message = "Unauthorized" };

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

    public async Task<ApiResponse<CoreUserData>> InsertCore(CoreUserData coreUserData)
    {
        try
        {
            CoreUserRequest coreUserRequest = await GetCoreUser();
            var coreDataRequest = new CoreDataRequest()
            {
                Data_Id = Guid.Empty,
                Data01 = coreUserData.Data01,
                Data02 = coreUserData.Data02,
                Data03 = coreUserData.Data03,
                CoreUser = coreUserRequest,
            };

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

    public async Task<ApiResponse<CoreUserData>> UpdateCore(CoreUserData coreUserData)
    {
        try
        {
            CoreUserRequest coreUserRequest = await GetCoreUser();
            var coreDataRequest = new CoreDataRequest()
            {
                Data_Id = coreUserData.Data_Id,
                Data01 = coreUserData.Data01,
                Data02 = coreUserData.Data02,
                Data03 = coreUserData.Data03,
                CoreUser = coreUserRequest,
            };

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

    public async Task<ApiResponse<object>> DeleteCore(Guid DataId)
    {
        try
        {
            CoreUserRequest coreUserRequest = await GetCoreUser();
            var coreDataDelete = new CoreDataDelete
            {
                Data_Id = DataId,
                CoreUser = coreUserRequest
            };

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

    private async Task<CoreUserRequest> GetCoreUser()
    {
        var userId = await _secureStorageService.GetUserIdAsync();
        var sqlToken = await _secureStorageService.GetSqlTokenAsync();

        return new CoreUserRequest
        {
            User_Id = Guid.Parse(userId),
            SqlToken = Guid.Parse(sqlToken)
        };
    }
}
