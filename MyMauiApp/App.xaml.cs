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

        return new Window(string.IsNullOrEmpty(token)
            ? new NavigationPage(new LoginPage())
            : new AppShell()
        );
    }
}