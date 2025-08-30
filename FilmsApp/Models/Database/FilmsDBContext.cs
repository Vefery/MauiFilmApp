using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using MauiTestApp.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MauiTestApp.Models.Database
{
    public class FilmsDBContext : DbContext
    {
        public DbSet<Film> Films => Set<Film>();
        public DbSet<Actor> Actors => Set<Actor>();
        public DbSet<FeaturedActor> FeaturedActors => Set<FeaturedActor>();
        public DbSet<Genre> Genres => Set<Genre>();
        public DbSet<HasGenre> HasGenre => Set<HasGenre>();

        private readonly ILogger<FilmsDBContext> logger;
        public FilmsDBContext(ILogger<FilmsDBContext> logger)
        {
            this.logger = logger;
            // Путь к файлу бд в локальной директории приложения
            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "filmsDb.db");

            // Если файла бд нет, то копирую его из ресурсов
            if (!File.Exists(dbPath))
            {
                CopyDatabaseFromAssets();
            }
        }

        // Конструктор для тестирования. По какой-то причине MAUI не работает
        // если передавать параметри через DI, поэтому рабочие параметры задаются
        // в OnConfiguring
        public FilmsDBContext(DbContextOptions<FilmsDBContext> options, ILogger<FilmsDBContext> logger) : base(options)
        {
            this.logger = logger;
            dbPath = string.Empty;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Для вспомогательных сущностей использую составной ключ вместо Id
            modelBuilder.Entity<FeaturedActor>()
                .HasKey(fa => new { fa.ActorId, fa.FilmId });
            modelBuilder.Entity<HasGenre>()
                .HasKey(hg => new { hg.GenreId, hg.FilmId });

            // Конфигурация связи многие-ко-многим фильмов и актеров
            modelBuilder.Entity<Film>()
                .HasMany(f => f.Actors)
                .WithMany(a => a.Films)
                .UsingEntity<FeaturedActor>();

            // Конфигурация связи многие-ко-многим фильмов и жанров
            modelBuilder.Entity<Film>()
                .HasMany(f => f.Genres)
                .WithMany(g => g.Films)
                .UsingEntity<HasGenre>();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // Использовать реальную бд только если не поступила иная конфигурация
            // (In-Memory бд для тестов через DI)
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
        }
        private void CopyDatabaseFromAssets()
        {
            using var stream = FileSystem.OpenAppPackageFileAsync("filmsDb.db").Result;
            using var dest = File.Create(dbPath);
            stream.CopyTo(dest);
        }
    }
}
