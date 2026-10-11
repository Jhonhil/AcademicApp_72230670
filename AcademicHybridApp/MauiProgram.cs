using AcademicHybridApp.Services;
using Microsoft.Extensions.Logging;

namespace AcademicHybridApp
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
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Logging.AddDebug();
#endif

            // Port disesuaikan dengan port aktif API (5057)
            string apiBaseUrl = "http://localhost:5057/";
            if (DeviceInfo.Platform == DevicePlatform.Android)
            {
                apiBaseUrl = "http://10.0.2.2:5057/"; // IP localhost khusus emulator Android
            }

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });
            builder.Services.AddScoped<DosenService>();
            builder.Services.AddScoped<MahasiswaService>();

            return builder.Build();
        }
    }
}