using Services.Interfaces;

namespace MyCapital.MAUI.Services;

public class Notifications : INotifications
{
    public async Task ShowAlert(string title, string message, string cancel)
    {
        if (Application.Current?.MainPage != null) 
            await Application.Current.MainPage.DisplayAlert(title, message, cancel);
    }

    public async Task<string?> ShowSheet(string title, string? cancel, string? destruction, params string[] buttons)
    {
        if (Application.Current?.MainPage != null)
        {
            return await Application.Current.MainPage.DisplayActionSheet(
                title,
                cancel,
                destruction,
                buttons);
        }

        return null;
    }
}