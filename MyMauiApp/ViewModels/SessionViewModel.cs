using MyMauiApp.Services;

namespace MyMauiApp.ViewModels;

public class SessionViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private readonly ISecureStorageService _storageService;

    private string _userId = string.Empty;
    private string _role = string.Empty;
    private string _sqlToken = string.Empty;
    private string _apiToken = string.Empty;
    private string _timeRemaining = "00:00";
    private Timer? _timer;
    private DateTime _expirationTime;

    public string UserId
    {
        get => _userId;
        set => SetProperty(ref _userId, value);
    }
    public string Role
    {
        get => _role;
        set => SetProperty(ref _role, value);
    }
    public string SqlToken
    {
        get => _sqlToken;
        set => SetProperty(ref _sqlToken, value);
    }
    public string ApiToken
    {
        get => _apiToken;
        set => SetProperty(ref _apiToken, value);
    }
    public string TimeRemaining
    {
        get => _timeRemaining;
        set => SetProperty(ref _timeRemaining, value);
    }

    public SessionViewModel(IAuthService authService, ISecureStorageService storageService)
    {
        _authService = authService;
        _storageService = storageService;
        Title = "Session";
        _ = LoadSessionDataAsync();
    }

    private async Task LoadSessionDataAsync()
    {
        UserId = await _storageService.GetUserIdAsync() ?? "N/A";
        Role = await _storageService.GetRoleAsync() ?? "N/A";
        SqlToken = await _storageService.GetSqlTokenAsync() ?? "N/A";
        ApiToken = await _storageService.GetApiTokenAsync() ?? "N/A";

        var expirationTime = await _storageService.GetExpirationTimeAsync();

        if (expirationTime.HasValue)
        {
            _expirationTime = expirationTime.Value;

            if (_expirationTime <= DateTime.Now)
            {
                await AutoLogoutAsync();
                return;
            }

            StartCountdown();
        }
    }

    private void StartCountdown()
    {
        _timer = new Timer(UpdateCountdown, null, 0, 1000);
    }

    private void UpdateCountdown(object? state)
    {
        var remaining = _expirationTime - DateTime.Now;

        if (remaining.TotalSeconds <= 0)
        {
            _timer?.Dispose();
            MainThread.BeginInvokeOnMainThread(async () => await AutoLogoutAsync());
            return;
        }

        TimeRemaining = $"{remaining.Minutes:D2}:{remaining.Seconds:D2}";
    }

    private async Task AutoLogoutAsync()
    {
        await _authService.LogoutAsync();
        Application.Current!.MainPage = new NavigationPage(
            Application.Current.Handler.MauiContext!.Services.GetRequiredService<Views.LoginPage>()
        );
    }
}
