using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using MauiTestApp.Models.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace MauiTestApp.Models.Database
{
    public class FilmsDBContext : DbContext
    {
        public DbSet<Film> Films => Set<Film>();
        public DbSet<Actor> Actors => Set<Actor>();
        public DbSet<FeaturedActor> FeaturedActors => Set<FeaturedActor>();
        public DbSet<Genre> Genres => Set<Genre>();

        private readonly string dbPath;
        public FilmsDBContext()
        {
            // Путь к файлу бд в локальной директории приложения
            dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "filmsDb.db");

            // Если файла бд нет, то копирую его из ресурсов
            if (!File.Exists(dbPath))
            {
                CopyDatabaseFromAssets();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Для вспомогательной сущность использую составной ключ вместо Id
            modelBuilder.Entity<FeaturedActor>()
                .HasKey(f => new { f.ActorId, f.FilmId });

            // Конфигурация связи многие-ко-многим
            modelBuilder.Entity<Film>()
                .HasMany(f => f.Actors)
                .WithMany(a => a.Films)
                .UsingEntity<FeaturedActor>();  
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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
