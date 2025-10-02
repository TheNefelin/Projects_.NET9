using MyMauiApp.Services;
using MyMauiApp.Views;

namespace MyMauiApp;

public partial class AppShell : Shell
{
    private IAuthService _authService;

    public AppShell()
    {
        InitializeComponent();
        Loaded += OnLoaded;
    }

    private void OnLoaded(object? sender, EventArgs e)
    {
        _authService = Handler.MauiContext!.Services.GetRequiredService<IAuthService>();
    }

    private async void OnLogoutClicked(object sender, EventArgs e)
    {
        await _authService.LogoutAsync();
        var loginPage = Handler.MauiContext!.Services.GetRequiredService<LoginPage>();
        Application.Current!.MainPage = new NavigationPage(loginPage);
    }
}
