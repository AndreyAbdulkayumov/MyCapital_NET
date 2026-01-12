namespace MyCapital.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            // Регистрация маршрута для дальнейшего перехода на эту страницу
            Routing.RegisterRoute(nameof(ErrorConnectionPage), typeof(ErrorConnectionPage));

            // Убираем подчеркивание для всех Entry
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
#if ANDROID
            handler.PlatformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Android.Graphics.Color.Transparent);
#elif IOS || MACCATALYST
            handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#elif WINDOWS
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
#endif
            });

            MainPage = new AppShell();
        }
    }
}
