using MyMauiApp.ViewModels;

namespace MyMauiApp.Views;

public partial class SessionPage : ContentPage
{
	public SessionPage(SessionViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
