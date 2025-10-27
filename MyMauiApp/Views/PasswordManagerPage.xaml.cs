using MyMauiApp.Models;
using MyMauiApp.ViewModels;

namespace MyMauiApp.Views;

public partial class PasswordManagerPage : ContentPage
{
    private PasswordManagerViewModel ViewModel => (PasswordManagerViewModel)BindingContext;

    public PasswordManagerPage(PasswordManagerViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private void OnItemTapped(object sender, EventArgs e)
    {
        var grid = (Grid)sender;
        var border = (Border)grid.Parent.Parent;
        var detailsSection = border.FindByName<VerticalStackLayout>("DetailsSection");
        detailsSection.IsVisible = !detailsSection.IsVisible;
    }

    private void OnEditClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = (CoreUserData)button.BindingContext;
        ViewModel.EditCommand.Execute(item);
    }

    private void OnDeleteClicked(object sender, EventArgs e)
    {
        var button = (Button)sender;
        var item = (CoreUserData)button.BindingContext;
        ViewModel.DeleteCommand.Execute(item);
    }
}
