using CommunityToolkit.Maui;
using MauiTestApp.Models.Database;
using MauiTestApp.Services;
using MauiTestApp.Services.Interfaces;
using MauiTestApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MauiTestApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
            builder.Logging.AddDebug();
#endif
            // Регистрирую контекст бд
            builder.Services.AddDbContext<FilmsDBContext>();
            builder.Services.AddTransient<IFilmsService, FilmsService>();
            builder.Services.AddSingleton<SearchViewModel>();
            builder.Services.AddSingleton<FilmDetailsViewModel>();

            return builder.Build();
        }
    }
}
