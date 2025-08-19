using Core;
using Core.RateSourse_RussianCentralBank;
using Microsoft.Extensions.Logging;
using MyCapital_Mobile_MAUI.Services;
using Services.Interfaces;
using ViewModels;

namespace MyCapital_Mobile_MAUI
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            // Models
            builder.Services.AddTransient<IRateSource, RussianCentralBank>();

            // ViewModels
            builder.Services.AddTransient<MainPage_VM>();

            // Views
            builder.Services.AddTransient<MainPage>();

            // Services
            builder.Services.AddTransient<INotifications, Notifications>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
