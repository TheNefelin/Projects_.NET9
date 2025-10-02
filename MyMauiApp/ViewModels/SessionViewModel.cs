using MyMauiApp.Services;
using System.Windows.Input;

namespace MyMauiApp.ViewModels;

public class SessionViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
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

    public ICommand LogoutCommand { get; }

    public SessionViewModel(IAuthService authService)
    {
        _authService = authService;
        Title = "Session";
        LogoutCommand = new Command(async () => await LogoutAsync());

        _ = LoadSessionDataAsync();
    }

    private async Task LoadSessionDataAsync()
    {
        UserId = await _authService.GetUserIdAsync() ?? "N/A";
        Role = await _authService.GetRoleAsync() ?? "N/A";
        SqlToken = await _authService.GetSqlTokenAsync() ?? "N/A";
        ApiToken = await _authService.GetJwtTokenAsync() ?? "N/A";

        var expireMin = await _authService.GetExpireMinAsync();
        _expirationTime = DateTime.Now.AddMinutes(expireMin);

        StartCountdown();
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

    private async Task LogoutAsync()
    {
        _timer?.Dispose();
        await _authService.LogoutAsync();
        Application.Current!.MainPage = new NavigationPage(
            Application.Current.Handler.MauiContext!.Services.GetRequiredService<Views.LoginPage>()
        );
    }
}
