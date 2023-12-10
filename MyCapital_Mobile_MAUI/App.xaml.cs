namespace MyCapital_Mobile_MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Регистрация маршрута для дальнейшего перехода на эту страницу
            Routing.RegisterRoute(nameof(ErrorConnectionPage), typeof(ErrorConnectionPage));

            MainPage = new AppShell();
        }
    }
}
