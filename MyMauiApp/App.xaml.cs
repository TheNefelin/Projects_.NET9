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

        if (string.IsNullOrEmpty(token))
        {
            var loginPage = Handler.MauiContext!.Services.GetRequiredService<LoginPage>();
            return new Window(new NavigationPage(loginPage));
        }
        else
        {
            return new Window(new AppShell());
        }
    }
}
