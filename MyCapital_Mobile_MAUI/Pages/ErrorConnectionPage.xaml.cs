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
            await Application.Current.MainPage.DisplayAlert("������",
                "�� ������� ������������ � ���������", "�K");

            return;
        }
        
        // ������� �� �������� ��� �������� ��������
        // URI ������ ../ ������� ������� ����� �� ����� (�������� pop)
        // ��������� ��� https://learn.microsoft.com/en-us/dotnet/maui/user-interface/pages/navigationpage?view=net-maui-7.0
        await Shell.Current.GoToAsync("../", false);
    }

}