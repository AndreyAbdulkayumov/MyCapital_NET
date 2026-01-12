namespace Services.Interfaces
{
    public interface INotifications
    {
        Task ShowAlert(string title, string message, string cancel);
        Task<string?> ShowSheet(string title, string? cancel, string? destruction, params string[] buttons);
    }
}
