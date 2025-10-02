using MyMauiApp.Models;
using MyMauiApp.Services;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MyMauiApp.ViewModels;

public class PasswordManagerViewModel : BaseViewModel
{
    private readonly ICoreService _coreService;
    private readonly IAuthService _authService;

    public ObservableCollection<CoreUserData> PasswordList { get; } = new();
    private ObservableCollection<CoreUserData> _allPasswords = new();
    private string _searchText = string.Empty;

    public string SearchText
    {
        get => _searchText;
        set
        {
            SetProperty(ref _searchText, value);
            FilterPasswords();
        }
    }

    public ICommand DownloadCommand { get; }
    public ICommand DecryptCommand { get; }
    public ICommand RegisterKeyCommand { get; }
    public ICommand CreateCommand { get; }
    public ICommand EditCommand { get; }
    public ICommand DeleteCommand { get; }

    public PasswordManagerViewModel(ICoreService coreService, IAuthService authService)
    {
        _coreService = coreService;
        _authService = authService;
        Title = "Password Manager";

        DownloadCommand = new Command(async () => await DownloadPasswordsAsync());
        DecryptCommand = new Command(async () => await DecryptAsync());
        RegisterKeyCommand = new Command(async () => await RegisterKeyAsync());
        CreateCommand = new Command(async () => await CreateAsync());
        EditCommand = new Command<CoreUserData>(async (item) => await EditAsync(item));
        DeleteCommand = new Command<CoreUserData>(async (item) => await DeleteAsync(item));
    }

    private void FilterPasswords()
    {
        PasswordList.Clear();

        var filtered = string.IsNullOrWhiteSpace(SearchText)
            ? _allPasswords
            : _allPasswords.Where(p => p.Data01.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        foreach (var item in filtered)
            PasswordList.Add(item);
    }

    private async Task DownloadPasswordsAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            var userId = await _authService.GetUserIdAsync();
            var sqlToken = await _authService.GetSqlTokenAsync();

            var request = new CoreUserRequest
            {
                User_Id = Guid.Parse(userId),
                SqlToken = Guid.Parse(sqlToken)
            };

            var result = await _coreService.GetAllCore(request);

            if (result.IsSuccess && result.Data != null)
            {
                _allPasswords.Clear();
                foreach (var item in result.Data)
                    _allPasswords.Add(item);

                FilterPasswords();
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", result.Message, "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task DecryptAsync()
    {
        var password = await Application.Current.MainPage.DisplayPromptAsync(
            "Desencriptar",
            "Ingresa tu contraseña:",
            placeholder: "Contraseña",
            maxLength: 100,
            keyboard: Keyboard.Text);

        if (string.IsNullOrWhiteSpace(password)) return;

        IsBusy = true;
        try
        {
            var userId = await _authService.GetUserIdAsync();
            var sqlToken = await _authService.GetSqlTokenAsync();

            var request = new CorePasswordRequest
            {
                Password = password,
                CoreUser = new CoreUserRequest
                {
                    User_Id = Guid.Parse(userId),
                    SqlToken = Guid.Parse(sqlToken)
                }
            };

            var result = await _coreService.GetCoreUserIVAsync(request);

            if (result.IsSuccess && result.Data != null)
            {
                await Application.Current.MainPage.DisplayAlert(
                    "IV Obtenido",
                    $"IV: {result.Data.IV}",
                    "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", result.Message, "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task RegisterKeyAsync()
    {
        var password = await Application.Current.MainPage.DisplayPromptAsync(
            "Registrar Contraseña",
            "Ingresa nueva contraseña:",
            placeholder: "Contraseña",
            maxLength: 100,
            keyboard: Keyboard.Text);

        if (string.IsNullOrWhiteSpace(password)) return;

        var confirmPassword = await Application.Current.MainPage.DisplayPromptAsync(
            "Confirmar Contraseña",
            "Confirma tu contraseña:",
            placeholder: "Contraseña",
            maxLength: 100,
            keyboard: Keyboard.Text);

        if (password != confirmPassword)
        {
            await Application.Current.MainPage.DisplayAlert("Error", "Las contraseñas no coinciden", "OK");
            return;
        }

        IsBusy = true;
        try
        {
            var userId = await _authService.GetUserIdAsync();
            var sqlToken = await _authService.GetSqlTokenAsync();

            var request = new CorePasswordRequest
            {
                Password = password,
                CoreUser = new CoreUserRequest {
                    User_Id = Guid.Parse(userId),
                    SqlToken = Guid.Parse(sqlToken)
                }
            };

            var result = await _coreService.RegisterPasswordAsync(request);

            if (result.IsSuccess)
            {
                await Application.Current.MainPage.DisplayAlert("Éxito", "Contraseña registrada", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", result.Message, "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task CreateAsync()
    {
        // TODO: Formulario para crear password
        await Application.Current.MainPage.DisplayAlert("Crear", "Formulario de creación", "OK");
    }

    private async Task EditAsync(CoreUserData item)
    {
        // TODO: Implementar edición (próximo paso)
        await Application.Current.MainPage.DisplayAlert("Editar", $"Editando: {item.Data01}", "OK");
    }

    private async Task DeleteAsync(CoreUserData item)
    {
        var confirm = await Application.Current.MainPage.DisplayAlert(
            "Eliminar",
            $"¿Eliminar {item.Data01}?",
            "Sí",
            "No");

        if (!confirm) return;

        IsBusy = true;
        try
        {
            var userId = await _authService.GetUserIdAsync();
            var sqlToken = await _authService.GetSqlTokenAsync();

            var request = new CoreDataDelete
            {
                Data_Id = item.Data_Id,
                CoreUser = new CoreUserRequest {
                    User_Id = Guid.Parse(userId),
                    SqlToken = Guid.Parse(sqlToken)
                }
            };

            var result = await _coreService.DeleteCore(request);

            if (result.IsSuccess)
            {
                PasswordList.Remove(item);
                await Application.Current.MainPage.DisplayAlert("Éxito", "Eliminado correctamente", "OK");
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Error", result.Message, "OK");
            }
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
