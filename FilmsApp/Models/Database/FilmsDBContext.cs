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

        public FilmsDBContext(DbContextOptions<FilmsDBContext> options, ILogger<FilmsDBContext> logger) : base(options)
        {
            this.logger = logger;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Для вспомогательных сущностей использую составной ключ
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
    }
}
