using MyMauiApp.Models;
using MyMauiApp.Services;
using MyMauiApp.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;
using Utils.Services;

namespace MyMauiApp.ViewModels;

public class PasswordManagerViewModel : BaseViewModel
{
    private readonly IEncryptionService _encryptionService;
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

    public PasswordManagerViewModel(IEncryptionService encryptionService, ICoreService coreService, IAuthService authService)
    {
        _encryptionService = encryptionService;
        _coreService = coreService;
        _authService = authService;
        Title = "Password Manager";

        DownloadCommand = new Command(
            execute: async () => await DownloadPasswordsAsync(),
            canExecute: () => !IsBusy);

        DecryptCommand = new Command(
            execute: async () => await DecryptAsync(),
            canExecute: () => !IsBusy);

        RegisterKeyCommand = new Command(
            execute: async () => await RegisterKeyAsync(),
            canExecute: () => !IsBusy);

        CreateCommand = new Command(
            execute: async () => await CreateAsync(),
            canExecute: () => !IsBusy);

        EditCommand = new Command<CoreUserData>(
            execute: async (item) => await EditAsync(item),
            canExecute: (item) => !IsBusy);

        DeleteCommand = new Command<CoreUserData>(
            execute: async (item) => await DeleteAsync(item),
            canExecute: (item) => !IsBusy);

        PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(IsBusy))
            {
                ((Command)DownloadCommand).ChangeCanExecute();
                ((Command)DecryptCommand).ChangeCanExecute();
                ((Command)RegisterKeyCommand).ChangeCanExecute();
                ((Command)CreateCommand).ChangeCanExecute();
                ((Command)EditCommand).ChangeCanExecute();
                ((Command)DeleteCommand).ChangeCanExecute();
            }
        };
    }

    private void FilterPasswords()
    {
        PasswordList.Clear();

        var filtered = string.IsNullOrWhiteSpace(SearchText)
            ? _allPasswords
            : _allPasswords.Where(p => !string.IsNullOrEmpty(p.Data01) && p.Data01.Contains(SearchText, StringComparison.OrdinalIgnoreCase));

        var sorted = filtered.OrderBy(p => p.Data01 ?? string.Empty);

        foreach (var item in sorted)
            PasswordList.Add(item);
    }

    private async Task DownloadPasswordsAsync()
    {
        if (IsBusy) return;
        IsBusy = true;

        try
        {
            var result = await _coreService.GetAllCore();

            if (result.IsSuccess && result.Data != null)
            {
                _allPasswords.Clear();
                foreach (var item in result.Data)
                    _allPasswords.Add(item);

                FilterPasswords();
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", result.Message, "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task RegisterKeyAsync()
    {
        var page = new PasswordPromptPage("Registrar Contraseña", requireConfirmation: true);
        await Shell.Current.Navigation.PushModalAsync(page);

        while (Shell.Current.Navigation.ModalStack.Contains(page))
            await Task.Delay(100);

        if (string.IsNullOrEmpty(page.Password)) return;

        IsBusy = true;
        try
        {
            var result = await _coreService.RegisterPasswordAsync(page.Password);

            if (result.IsSuccess)
            {
                await Shell.Current.DisplayAlert("Éxito", "Contraseña registrada", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", result.Message, "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task DecryptAsync()
    {
        try
        {

            if (IsBusy) return; // Doble protección
            IsBusy = true;

            var page = new PasswordPromptPage("Desencriptar");
            await Shell.Current.Navigation.PushModalAsync(page);

            while (Shell.Current.Navigation.ModalStack.Contains(page))
                await Task.Delay(100);

            if (string.IsNullOrEmpty(page.Password)) return;

            var result = await _coreService.GetCoreUserIVAsync(page.Password);

            if (result.IsSuccess && result.Data != null)
            {
                var currentIV = result.Data.IV;

                foreach (var item in PasswordList)
                {
                    try
                    {
                        item.Data01 = _encryptionService.Decrypt(item.Data01, page.Password, currentIV);
                        item.Data02 = _encryptionService.Decrypt(item.Data02, page.Password, currentIV);
                        item.Data03 = _encryptionService.Decrypt(item.Data03, page.Password, currentIV);
                    }
                    catch
                    {
                        // Ignorar errores de desencriptación para elementos individuales
                    }
                }

                FilterPasswords();

                await Shell.Current.DisplayAlert("IV Obtenido", $"IV: {result.Data.IV}", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", result.Message, "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task CreateAsync()
    {
        var page = new CreateEditPasswordPage();
        await Shell.Current.Navigation.PushModalAsync(page);

        await page.Dispatcher.DispatchAsync(async () =>
        {
            while (Shell.Current.Navigation.ModalStack.Contains(page))
                await Task.Delay(100);

            if (page.Result != null)
                await SavePasswordAsync(page.Result);
        });
    }

    private async Task SavePasswordAsync(CoreUserData data)
    {
        IsBusy = true;
        try
        {
            var result = data.Data_Id == Guid.Empty
                ? await _coreService.InsertCore(data)
                : await _coreService.UpdateCore(data);

            if (result.IsSuccess)
            {
                await DownloadPasswordsAsync();
                await Shell.Current.DisplayAlert("Éxito", "Guardado correctamente", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", result.Message, "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }

    private async Task EditAsync(CoreUserData item)
    {
        var page = new CreateEditPasswordPage(item);
        await Shell.Current.Navigation.PushModalAsync(page);

        await page.Dispatcher.DispatchAsync(async () =>
        {
            while (Shell.Current.Navigation.ModalStack.Contains(page))
                await Task.Delay(100);

            if (page.Result != null)
                await SavePasswordAsync(page.Result);
        });
    }

    private async Task DeleteAsync(CoreUserData item)
    {
        var confirm = await Shell.Current.DisplayAlert(
            "Eliminar",
            $"¿Eliminar {item.Data01}?",
            "Sí",
            "No");

        if (!confirm) return;

        IsBusy = true;
        try
        {
            var result = await _coreService.DeleteCore(item.Data_Id);

            if (result.IsSuccess)
            {
                PasswordList.Remove(item);
                await Shell.Current.DisplayAlert("Éxito", "Eliminado correctamente", "OK");
            }
            else
            {
                await Shell.Current.DisplayAlert("Error", result.Message, "OK");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
        }
        finally
        {
            IsBusy = false;
        }
    }
}
