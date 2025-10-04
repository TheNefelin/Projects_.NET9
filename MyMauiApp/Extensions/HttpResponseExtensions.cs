using System.Net;

namespace MyMauiApp.Extensions;

public static class HttpResponseExtensions
{
    public static async Task<bool> HandleUnauthorizedAsync(this HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            SecureStorage.RemoveAll();

            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Application.Current.MainPage.DisplayAlert(
                    "Sesión Expirada",
                    "Tu sesión ha expirado. Inicia sesión nuevamente.",
                    "OK");

                Application.Current.MainPage = new NavigationPage(
                    Application.Current.Handler.MauiContext!.Services.GetRequiredService<Views.LoginPage>()
                );
            });

            return true;
        }
        return false;
    }
}
