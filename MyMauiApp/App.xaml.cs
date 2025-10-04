using MyMauiApp.Views;

namespace MyMauiApp;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        var token = SecureStorage.GetAsync("ApiToken").Result;
        var expirationStr = SecureStorage.GetAsync("ExpirationTime").Result;

        bool isExpired = false;
        if (!string.IsNullOrEmpty(expirationStr))
        {
            var expirationTime = DateTime.Parse(expirationStr);
            isExpired = expirationTime <= DateTime.Now;
        }

        if (string.IsNullOrEmpty(token) || isExpired)
        {
            if (isExpired) SecureStorage.RemoveAll();

            var loginPage = Handler.MauiContext!.Services.GetRequiredService<LoginPage>();
            return new Window(new NavigationPage(loginPage));
        }
        else
        {
            return new Window(new AppShell());
        }
    }
}
