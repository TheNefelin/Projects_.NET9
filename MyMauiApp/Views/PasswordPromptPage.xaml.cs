namespace MyMauiApp.Views;

public partial class PasswordPromptPage : ContentPage
{
    public string? Password { get; private set; }
    private readonly bool _requireConfirmation;
    private bool _isFirstPassword = true;
    private string? _firstPassword;
    private bool _isClosing = false; // Agregar flag

    public PasswordPromptPage(string title, bool requireConfirmation = false)
    {
        InitializeComponent();
        TitleLabel.Text = title;
        _requireConfirmation = requireConfirmation;
    }

    private async void OnAcceptClicked(object sender, EventArgs e)
    {
        if (_isClosing) return;
        await ProcessPassword();
    }

    private async void OnPasswordCompleted(object sender, EventArgs e)
    {
        if (_isClosing) return;
        await ProcessPassword();
    }

    private async Task ProcessPassword()
    {
        if (string.IsNullOrWhiteSpace(PasswordEntry.Text))
        {
            ErrorLabel.Text = "La contraseña es requerida";
            ErrorLabel.IsVisible = true;
            return;
        }

        if (_requireConfirmation && _isFirstPassword)
        {
            _firstPassword = PasswordEntry.Text;
            _isFirstPassword = false;
            TitleLabel.Text = "Confirmar Contraseña";
            PasswordEntry.Text = string.Empty;
            PasswordEntry.Focus();
            return;
        }

        if (_requireConfirmation && !_isFirstPassword)
        {
            if (PasswordEntry.Text != _firstPassword)
            {
                ErrorLabel.Text = "Las contraseñas no coinciden";
                ErrorLabel.IsVisible = true;
                PasswordEntry.Text = string.Empty;
                _isFirstPassword = true;
                _firstPassword = null;
                TitleLabel.Text = "Registrar Contraseña";
                return;
            }
        }

        Password = _requireConfirmation ? _firstPassword : PasswordEntry.Text;
        await CloseModal();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        if (_isClosing) return;
        Password = null;
        await CloseModal();
    }

    private async Task CloseModal()
    {
        if (_isClosing) return;
        _isClosing = true;

        try
        {
            await Navigation.PopModalAsync();
        }
        catch (InvalidOperationException)
        {
            // Ya cerrado, ignorar
        }
    }
}
