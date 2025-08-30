using CommunityToolkit.Maui;
using MauiTestApp.Models.Database;
using MauiTestApp.Models.Repositories;
using MauiTestApp.Models.Repositories.Interfaces;
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
            builder.Services.AddDbContext<FilmsDBContext>(options =>
            {
                // Путь к файлу бд в локальной директории приложения
                string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "filmsDb.db");

                // Если файла бд нет, то копирую его из ресурсов
                if (!File.Exists(dbPath))
                {
                    using var stream = FileSystem.OpenAppPackageFileAsync("filmsDb.db").Result;
                    using var dest = File.Create(dbPath);
                    stream.CopyTo(dest);
                }
                options.UseSqlite($"Data Source={dbPath}");
            });
            builder.Services.AddTransient<IFilmsRepository, FilmsRepository>();
            builder.Services.AddTransient<IFilmsService, FilmsService>();
            builder.Services.AddSingleton<SearchViewModel>();
            builder.Services.AddSingleton<FilmDetailsViewModel>();

            return builder.Build();
        }
    }
}
