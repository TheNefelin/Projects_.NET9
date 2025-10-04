using MyMauiApp.Models;
using MyMauiApp.Services;
using MyMauiApp.Views;
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
        var page = new PasswordPromptPage("Registrar Contraseña", requireConfirmation: true);
        await Application.Current.MainPage.Navigation.PushModalAsync(page);

        while (Application.Current.MainPage.Navigation.ModalStack.Contains(page))
            await Task.Delay(100);

        if (string.IsNullOrEmpty(page.Password)) return;

        IsBusy = true;
        try
        {
            var result = await _coreService.RegisterPasswordAsync(page.Password);

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

    private async Task DecryptAsync()
    {
        try
        {

            if (IsBusy) return; // Doble protección
            IsBusy = true;

            var page = new PasswordPromptPage("Desencriptar");
            await Application.Current.MainPage.Navigation.PushModalAsync(page);

            while (Application.Current.MainPage.Navigation.ModalStack.Contains(page))
                await Task.Delay(100);

            if (string.IsNullOrEmpty(page.Password)) return;

            var result = await _coreService.GetCoreUserIVAsync(page.Password);

            if (result.IsSuccess && result.Data != null)
            {
                await Application.Current.MainPage.DisplayAlert("IV Obtenido", $"IV: {result.Data.IV}", "OK");
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
        var page = new CreateEditPasswordPage();
        await Application.Current.MainPage.Navigation.PushModalAsync(page);

        await page.Dispatcher.DispatchAsync(async () =>
        {
            while (Application.Current.MainPage.Navigation.ModalStack.Contains(page))
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
                await Application.Current.MainPage.DisplayAlert("Éxito", "Guardado correctamente", "OK");
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

    private async Task EditAsync(CoreUserData item)
    {
        var page = new CreateEditPasswordPage(item);
        await Application.Current.MainPage.Navigation.PushModalAsync(page);

        await page.Dispatcher.DispatchAsync(async () =>
        {
            while (Application.Current.MainPage.Navigation.ModalStack.Contains(page))
                await Task.Delay(100);

            if (page.Result != null)
                await SavePasswordAsync(page.Result);
        });
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
            var result = await _coreService.DeleteCore(item.Data_Id);

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
