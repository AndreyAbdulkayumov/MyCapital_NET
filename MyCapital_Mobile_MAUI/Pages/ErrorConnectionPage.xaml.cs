namespace MyCapital_Mobile_MAUI;

public partial class ErrorConnectionPage : ContentPage
{
	public ErrorConnectionPage()
	{
		InitializeComponent();
	}

    private async void Button_TryAgain_Clicked(object sender, EventArgs e)
    {
        if (Connectivity.Current.NetworkAccess != NetworkAccess.Internet)
        {
            await Application.Current.MainPage.DisplayAlert("Ошибка",
                "Не удалось подключиться к Интернету", "ОK");

            return;
        }
        
        // Перейти на страницу без анимации перехода
        // URI равный ../ означет переход вверх по стеку (операция pop)
        // Подробнее тут https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pages/navigationpage?view=net-maui-7.0
        await Shell.Current.GoToAsync("../", false);
    }

}