using MyMauiApp.Models;
using MyMauiApp.Services;
using System.Windows.Input;

namespace MyMauiApp.ViewModels;

public class LoginViewModel : BaseViewModel
{
    private readonly IAuthService _authService;
    private string _email = string.Empty;
    private string _password = string.Empty;
    private string _errorMessage = string.Empty;

    public string Email
    {
        get => _email;
        set => SetProperty(ref _email, value);
    }

    public string Password
    {
        get => _password;
        set => SetProperty(ref _password, value);
    }

    public string ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public ICommand LoginCommand { get; }

    public LoginViewModel(IAuthService authService)
    {
        _authService = authService;
        LoginCommand = new Command(
            execute: async () => await LoginAsync(),
            canExecute: () => !IsBusy && !string.IsNullOrWhiteSpace(Email) && !string.IsNullOrWhiteSpace(Password)
        );

        Title = "Login";

        PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(Email) || e.PropertyName == nameof(Password) || e.PropertyName == nameof(IsBusy))
                ((Command)LoginCommand).ChangeCanExecute();
        };
    }

    private async Task LoginAsync()
    {
        if (string.IsNullOrWhiteSpace(Email) || string.IsNullOrWhiteSpace(Password))
        {
            ErrorMessage = "Email y password requeridos";
            return;
        }

        IsBusy = true;
        ErrorMessage = string.Empty;

        var credentials = new AuthUserLogin { Email = Email, Password = Password };
        var result = await _authService.LoginAsync(credentials);

        IsBusy = false;

        if (result.IsSuccess)
        {
            Application.Current!.MainPage = new AppShell();
        }
        else
        {
            ErrorMessage = result.Message ?? "Error al iniciar sesión";
        }
    }
}
