using MyMauiApp.Models;

namespace MyMauiApp.Views;

public partial class CreateEditPasswordPage : ContentPage
{
    public CoreUserData? Result { get; private set; }
    private readonly CoreUserData? _editingItem;
    private bool _isClosing = false;

    public CreateEditPasswordPage(CoreUserData? itemToEdit = null)
    {
        InitializeComponent();
        _editingItem = itemToEdit;

        if (_editingItem != null)
        {
            Title = "Editar Password";
            Data01Entry.Text = _editingItem.Data01;
            Data02Entry.Text = _editingItem.Data02;
            Data03Entry.Text = _editingItem.Data03;
        }
        else
        {
            Title = "Crear Password";
        }
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        if (_isClosing) return;

        if (string.IsNullOrWhiteSpace(Data01Entry.Text))
        {
            ErrorLabel.Text = "Data01 es requerido";
            ErrorLabel.IsVisible = true;
            return;
        }

        Result = new CoreUserData
        {
            Data_Id = _editingItem?.Data_Id ?? Guid.Empty,
            Data01 = Data01Entry.Text,
            Data02 = Data02Entry.Text ?? string.Empty,
            Data03 = Data03Entry.Text ?? string.Empty
        };

        await CloseModal();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        if (_isClosing) return;
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
