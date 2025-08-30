using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MauiTestApp.Models.Database;
using MauiTestApp.Models.Database.Entities;
using MauiTestApp.Models.Repositories;
using MauiTestApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;

namespace FilmsApp.Tests.RepositoryTests
{
    public class FilmsRepositoryTests
    {
        // Тест получение жанров из бд
        [Fact]
        public async Task GetAllGenresAsync_NoGenres_ReturnsEmptyCollection()
        {
            // Arrange
            var repository = new FilmsRepository(Mock.Of<ILogger<FilmsRepository>>(), await CreateMockDb());

            // Act
            var results = await repository.GetAllGenresAsync();

            // Assert
            results.Should().NotBeNull();
            results.Should().BeEquivalentTo(["Драма", "Триллер"]);
        }
        // Тест на обработку невалидного ID фильма
        [Fact]
        public async Task GetFilmDetailsAsync_FilmNotFound_ReturnsNull()
        {
            // Arrange
            int filmId = 333;
            var repository = new FilmsRepository(Mock.Of<ILogger<FilmsRepository>>(), await CreateMockDb());

            // Act
            var results = await repository.GetFilmDetailsAsync(filmId);

            // Assert
            results.Should().BeNull();
        }
        // Тест получение информации о фильме из бд
        [Fact]
        public async Task GetFilmDetailsAsync_FilmFound_ReturnsFilm()
        {
            // Arrange
            int filmId = 1;
            var repository = new FilmsRepository(Mock.Of<ILogger<FilmsRepository>>(), await CreateMockDb());
            var expectedFilm = new Film { Id = 1, Name = "Фильм", NameNormalized = "ФИЛЬМ", Description = "Описание", PosterPath = "poster1.png" };

            // Act
            var results = await repository.GetFilmDetailsAsync(filmId);

            // Assert
            results.Should().NotBeNull()
                .And.BeOfType<Film?>();
            results.Should().BeEquivalentTo(expectedFilm, options => options
                .Excluding(f => f.Actors)
                .Excluding(f => f.Genres));
            results.Actors.Should().BeEquivalentTo([new Actor() { Id = 1, Name = "Иванов Иван", NameNormalized = "ИВАНОВ ИВАН" }], options => options
                .Excluding(a => a.Films));
            results.Genres.Should().BeEquivalentTo([new Genre() { Id = 1, GenreName = "Драма" }], options => options
                .Excluding(g => g.Films));
        }
        // Тест на поиск по актеру с пустым фильтром
        [Fact]
        public async Task SearchFilmsByActorAsync_EmptyFilter_ReturnsAllFilms()
        {
            // Arrange
            SearchFilter filter = new(string.Empty, []);
            var repository = new FilmsRepository(Mock.Of<ILogger<FilmsRepository>>(), await CreateMockDb());
            ICollection<Film> expectedFilms = [
                new Film { Id = 1, Name = "Фильм", NameNormalized = "ФИЛЬМ", Description = "Описание", PosterPath = "poster1.png"},
                new Film { Id = 2, Name = "Кино", NameNormalized = "КИНО", Description = "Описание", PosterPath = "poster2.png"},
                new Film { Id = 3, Name = "Кинофильм", NameNormalized = "КИНОФИЛЬМ", Description = "Описание", PosterPath = "poster3.png"}
                ];

            // Act
            var results = await repository.SearchFilmsByActorAsync(filter);

            // Assert
            results.Should().NotBeNull()
                .And.BeAssignableTo<ICollection<Film>>();
            results.Should().BeEquivalentTo(expectedFilms);
        }
        // Тест на поиск по актеру в пустой бд
        [Fact]
        public async Task SearchFilmsByActorAsync_EmptyBD_ReturnsAllFilms()
        {
            // Arrange
            SearchFilter filter = new(string.Empty, []);
            var emptyRepository = new FilmsRepository(Mock.Of<ILogger<FilmsRepository>>(), await CreateMockDb(false));

            // Act
            var results = await emptyRepository.SearchFilmsByActorAsync(filter);

            // Assert
            results.Should().NotBeNull()
                .And.BeAssignableTo<ICollection<Film>>();
            results.Should().BeEmpty();
        }
        // Тест на поиск по актеру с непустым запросом
        [Fact]
        public async Task SearchFilmsByActorAsync_QueryFilter_ReturnsFilms()
        {
            // Arrange
            SearchFilter filter = new("ИВАН", []);
            var repository = new FilmsRepository(Mock.Of<ILogger<FilmsRepository>>(), await CreateMockDb());
            ICollection<Film> expectedFilms = [
                new Film { Id = 1, Name = "Фильм", NameNormalized = "ФИЛЬМ", Description = "Описание", PosterPath = "poster1.png"},
                new Film { Id = 3, Name = "Кинофильм", NameNormalized = "КИНОФИЛЬМ", Description = "Описание", PosterPath = "poster3.png"}
                ];

            // Act
            var results = await repository.SearchFilmsByActorAsync(filter);

            // Assert
            results.Should().NotBeNull()
                .And.BeAssignableTo<ICollection<Film>>();
            results.Should().BeEquivalentTo(expectedFilms);
        }
        // Тест на поиск по актеру с необходимыми жанрами
        [Fact]
        public async Task SearchFilmsByActorAsync_GenreFilter_ReturnsFilms()
        {
            // Arrange
            SearchFilter filter = new(string.Empty, ["Драма"]);
            var repository = new FilmsRepository(Mock.Of<ILogger<FilmsRepository>>(), await CreateMockDb());
            ICollection<Film> expectedFilms = [
                new Film { Id = 1, Name = "Фильм", NameNormalized = "ФИЛЬМ", Description = "Описание", PosterPath = "poster1.png"},
                new Film { Id = 3, Name = "Кинофильм", NameNormalized = "КИНОФИЛЬМ", Description = "Описание", PosterPath = "poster3.png"}
                ];

            // Act
            var results = await repository.SearchFilmsByActorAsync(filter);

            // Assert
            results.Should().NotBeNull()
                .And.BeAssignableTo<ICollection<Film>>();
            results.Should().BeEquivalentTo(expectedFilms, 
                options => options
                    .Excluding(f => f.Genres)
                    .Excluding(f => f.Actors));
        }

        // Тест на поиск по названию с пустым фильтром
        [Fact]
        public async Task SearchFilmsByNameAsync_EmptyFilter_ReturnsAllFilms()
        {
            // Arrange
            SearchFilter filter = new(string.Empty, []);
            var repository = new FilmsRepository(Mock.Of<ILogger<FilmsRepository>>(), await CreateMockDb());
            ICollection<Film> expectedFilms = [
                new Film { Id = 1, Name = "Фильм", NameNormalized = "ФИЛЬМ", Description = "Описание", PosterPath = "poster1.png"},
                new Film { Id = 2, Name = "Кино", NameNormalized = "КИНО", Description = "Описание", PosterPath = "poster2.png"},
                new Film { Id = 3, Name = "Кинофильм", NameNormalized = "КИНОФИЛЬМ", Description = "Описание", PosterPath = "poster3.png"}
                ];

            // Act
            var results = await repository.SearchFilmsByNameAsync(filter);

            // Assert
            results.Should().NotBeNull()
                .And.BeAssignableTo<ICollection<Film>>();
            results.Should().BeEquivalentTo(expectedFilms);
        }
        // Тест на поиск по имени в пустой бд
        [Fact]
        public async Task SearchFilmsByNameAsync_EmptyBD_ReturnsAllFilms()
        {
            // Arrange
            SearchFilter filter = new(string.Empty, []);
            var emptyRepository = new FilmsRepository(Mock.Of<ILogger<FilmsRepository>>(), await CreateMockDb(false));

            // Act
            var results = await emptyRepository.SearchFilmsByNameAsync(filter);

            // Assert
            results.Should().NotBeNull()
                .And.BeAssignableTo<ICollection<Film>>();
            results.Should().BeEmpty();
        }
        // Тест на поиск по названию с непустым запросом
        [Fact]
        public async Task SearchFilmsByNameAsync_QueryFilter_ReturnsFilms()
        {
            // Arrange
            SearchFilter filter = new("ФИЛЬМ", []);
            var repository = new FilmsRepository(Mock.Of<ILogger<FilmsRepository>>(), await CreateMockDb());
            ICollection<Film> expectedFilms = [
                new Film { Id = 1, Name = "Фильм", NameNormalized = "ФИЛЬМ", Description = "Описание", PosterPath = "poster1.png"},
                new Film { Id = 3, Name = "Кинофильм", NameNormalized = "КИНОФИЛЬМ", Description = "Описание", PosterPath = "poster3.png"}
                ];

            // Act
            var results = await repository.SearchFilmsByNameAsync(filter);

            // Assert
            results.Should().NotBeNull()
                .And.BeAssignableTo<ICollection<Film>>();
            results.Should().BeEquivalentTo(expectedFilms);
        }
        // Тест на поиск по названию с необходимыми жанрами
        [Fact]
        public async Task SearchFilmsByNameAsync_GenreFilter_ReturnsFilms()
        {
            // Arrange
            SearchFilter filter = new(string.Empty, ["Драма"]);
            var repository = new FilmsRepository(Mock.Of<ILogger<FilmsRepository>>(), await CreateMockDb());
            ICollection<Film> expectedFilms = [
                new Film { Id = 1, Name = "Фильм", NameNormalized = "ФИЛЬМ", Description = "Описание", PosterPath = "poster1.png"},
                new Film { Id = 3, Name = "Кинофильм", NameNormalized = "КИНОФИЛЬМ", Description = "Описание", PosterPath = "poster3.png"}
                ];

            // Act
            var results = await repository.SearchFilmsByNameAsync(filter);

            // Assert
            results.Should().NotBeNull()
                .And.BeAssignableTo<ICollection<Film>>();
            results.Should().BeEquivalentTo(expectedFilms,
                options => options
                    .Excluding(f => f.Genres)
                    .Excluding(f => f.Actors));
        }


        // Вспомогательные методы
        private static async Task<FilmsDBContext> CreateMockDb(bool populate = true)
        {
            var options = new DbContextOptionsBuilder<FilmsDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            var dbContext = new FilmsDBContext(options, Mock.Of<ILogger<FilmsDBContext>>());

            dbContext.Database.EnsureCreated();
            if (populate)
                await PopulateMockDb(dbContext);
            await dbContext.SaveChangesAsync();

            return dbContext;
        }
        private static async Task PopulateMockDb(FilmsDBContext db)
        {
            await db.Actors.AddRangeAsync([
                new Actor() { Id = 1, Name = "Иванов Иван", NameNormalized = "ИВАНОВ ИВАН"},
                new Actor() { Id = 2, Name = "Петров Петр", NameNormalized = "ПЕТРОВ ПЕТР"},
                ]);
            await db.Films.AddRangeAsync([
                new Film { Id = 1, Name = "Фильм", NameNormalized = "ФИЛЬМ", Description = "Описание", PosterPath = "poster1.png"},
                new Film { Id = 2, Name = "Кино", NameNormalized = "КИНО", Description = "Описание", PosterPath = "poster2.png"},
                new Film { Id = 3, Name = "Кинофильм", NameNormalized = "КИНОФИЛЬМ", Description = "Описание", PosterPath = "poster3.png"}
                ]);
            await db.Genres.AddRangeAsync([
                new Genre() { Id = 1, GenreName = "Драма"},
                new Genre() { Id = 2, GenreName = "Триллер"}
                ]);
            await db.FeaturedActors.AddRangeAsync([
                new FeaturedActor() { ActorId = 1, FilmId = 1},
                new FeaturedActor() { ActorId = 2, FilmId = 2},
                new FeaturedActor() { ActorId = 1, FilmId = 3},
                new FeaturedActor() { ActorId = 2, FilmId = 3},
                ]);
            await db.HasGenre.AddRangeAsync([
                new HasGenre() { FilmId = 1, GenreId = 1 },
                new HasGenre() { FilmId = 2, GenreId = 2 },
                new HasGenre() { FilmId = 3, GenreId = 1 },
                new HasGenre() { FilmId = 3, GenreId = 2 },
                ]);
        }
    }
}
