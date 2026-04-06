using Microsoft.Maui.Controls.Hosting;
using Microsoft.Maui.Hosting;
using BarcodeScanning;
using TouristGuide.Maui.Services;
using TouristGuide.Maui.ViewModels;
using TouristGuide.Maui.Views;

namespace TouristGuide.Maui;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseBarcodeScanning()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Services
        builder.Services.AddSingleton<ApiService>();
        builder.Services.AddSingleton<GeofenceService>();
        builder.Services.AddSingleton<NarrationService>();

        // ViewModels
        builder.Services.AddSingleton<MainViewModel>();

        // Views
        builder.Services.AddTransient<TourSelectionPage>();
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<QrScanPage>();
        builder.Services.AddTransient<ImageViewerPage>();

        return builder.Build();
    }
}
