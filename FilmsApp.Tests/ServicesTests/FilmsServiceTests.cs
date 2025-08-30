using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MauiTestApp.DTOs;
using MauiTestApp.Models.Database.Entities;
using MauiTestApp.Models.Repositories.Interfaces;
using MauiTestApp.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace FilmsApp.Tests.ServicesTests
{
    public class FilmsServiceTests
    {
        private Mock<IFilmsRepository> mockRepo;
        private ILogger<FilmsService> mockLogger;
        private FilmsService service;
        private ICollection<Film> films;
        public FilmsServiceTests()
        {
            mockLogger = Mock.Of<ILogger<FilmsService>>();
            mockRepo = new Mock<IFilmsRepository>();
            service = new FilmsService(mockRepo.Object, mockLogger);
            films = [
                new()
                {
                    Id = 1,
                    Name = "Фильм 1",
                    NameNormalized = "ФИЛЬМ 1",
                    Description = "Описание фильма 1",
                    Actors = [],
                    Genres = [],
                    PosterPath = "poster1.png",
                    ReleaseDate = new DateOnly(2025, 1, 1)
                },
                new()
                {
                    Id = 1,
                    Name = "Фильм 2",
                    NameNormalized = "ФИЛЬМ 2",
                    Description = "Описание фильма 2",
                    Actors = [],
                    Genres = [],
                    PosterPath = "poster2.png",
                    ReleaseDate = new DateOnly(2025, 1, 2)
                }
            ];
        }
        // Тестирование поиска по имени актера
        // Тест на обработку пустого списка фильмов
        [Fact]
        public async Task SearchFilmsByActorAsync_NoFilmsInDB_ReturnEmptyCollection()
        {
            // Arrange
            var filter = new SearchFilter(string.Empty, []);
            mockRepo.Setup(r => r.SearchFilmsByActorAsync(It.IsAny<SearchFilter>())).ReturnsAsync([]);

            // Act
            var results = await service.SearchFilmsByActorAsync(filter);

            //Assert
            results.Should().NotBeNull();
            results.Should().BeAssignableTo<ICollection<FilmEntryDTO>>();
            results.Should().BeEmpty();
        }
        // Тест на обработку непустого списка фильмов
        [Fact]
        public async Task SearchFilmsByActorAsync_FilmsReturned_ReturnsFilmsDTOs()
        {
            // Arrange
            var filter = new SearchFilter(string.Empty, []);
            mockRepo.Setup(r => r.SearchFilmsByActorAsync(It.IsAny<SearchFilter>()))
                .ReturnsAsync([.. films]);

            // Act
            var results = await service.SearchFilmsByActorAsync(filter);

            //Assert
            results.Should().NotBeNullOrEmpty()
                .And.HaveCount(2);
            results.Should().BeAssignableTo<ICollection<FilmEntryDTO>>();
        }
        // Тест на корректную передачу фильтра
        [Fact]
        public async Task SearchFilmsByActorAsync_FilterNotEmpty_FilterPassedThrough()
        {
            // Arrange
            var filter = new SearchFilter("АКТЕРА 2", ["Драма", "История"]);
            SearchFilter? capturedFilter = null;
            mockRepo.Setup(r => r.SearchFilmsByActorAsync(It.IsAny<SearchFilter>()))
                .Callback<SearchFilter>(c => capturedFilter = c)
                .ReturnsAsync([]);

            // Act
            var results = await service.SearchFilmsByActorAsync(filter);

            //Assert
            capturedFilter.Should().NotBeNull()
                .And.BeEquivalentTo(filter);
        }

        // Тестирование поиска по названию фильма
        // Тест на обработку пустого списка фильмов
        [Fact]
        public async Task SearchFilmsByNameAsync_NoFilmsInDB_ReturnEmptyCollection()
        {
            // Arrange
            var filter = new SearchFilter(string.Empty, []);
            mockRepo.Setup(r => r.SearchFilmsByNameAsync(It.IsAny<SearchFilter>())).ReturnsAsync([]);

            // Act
            var results = await service.SearchFilmsByNameAsync(filter);

            //Assert
            results.Should().NotBeNull();
            results.Should().BeAssignableTo<ICollection<FilmEntryDTO>>();
            results.Should().BeEmpty();
        }
        // Тест на обработку непустого списка фильмов
        [Fact]
        public async Task SearchFilmsByNameAsync_FilmsReturned_ReturnsFilmsDTOs()
        {
            // Arrange
            var filter = new SearchFilter(string.Empty, []);
            mockRepo.Setup(r => r.SearchFilmsByNameAsync(It.IsAny<SearchFilter>()))
                .ReturnsAsync([.. films]);

            // Act
            var results = await service.SearchFilmsByNameAsync(filter);

            //Assert
            results.Should().NotBeNullOrEmpty()
                .And.HaveCount(2);
            results.Should().BeAssignableTo<ICollection<FilmEntryDTO>>();
        }
        // Тест на корректную передачу фильтра
        [Fact]
        public async Task SearchFilmsByNameAsync_FilterNotEmpty_FilterPassedThrough()
        {
            // Arrange
            var filter = new SearchFilter("ФИЛЬМ 2", ["Драма", "История"]);
            SearchFilter? capturedFilter = null;
            mockRepo.Setup(r => r.SearchFilmsByNameAsync(It.IsAny<SearchFilter>()))
                .Callback<SearchFilter>(c => capturedFilter = c)
                .ReturnsAsync([]);

            // Act
            var results = await service.SearchFilmsByNameAsync(filter);

            //Assert
            capturedFilter.Should().NotBeNull()
                .And.BeEquivalentTo(filter);
        }

        // Тест на получение пустого списка жанров
        [Fact]
        public async Task GetAllGenresAsync_NoGenres_ReturnsEmptyCollection()
        {
            // Arrange
            mockRepo.Setup(r => r.GetAllGenresAsync()).ReturnsAsync([]);

            // Act
            var results = await service.GetAllGenresAsync();

            // Assert
            results.Should().NotBeNull();
            results.Should().BeAssignableTo<ICollection<string>>();
            results.Should().BeEmpty();
        }
        // Тест на получение списка жанров
        [Fact]
        public async Task GetAllGenresAsync_GetsCollection_ReturnsCollection()
        {
            // Arrange
            mockRepo.Setup(r => r.GetAllGenresAsync()).ReturnsAsync([
                "Драма",
                "История"
                ]);

            // Act
            var results = await service.GetAllGenresAsync();

            // Assert
            results.Should().NotBeNull();
            results.Should().BeAssignableTo<ICollection<string>>();
            results.Should().Equal("Драма", "История");
        }
        // Тест на обработку null
        [Fact]
        public async Task GetFilmDetailsAsync_GetsNull_ReturnsNull()
        {
            // Arrange
            mockRepo.Setup(r => r.GetFilmDetailsAsync(It.IsAny<int>())).ReturnsAsync((Film?)null);

            // Act
            var result = await service.GetFilmDetailsAsync(0);

            // Assert
            result.Should().BeNull();
        }
        // Тест на правильность конвертации в DTO
        [Fact]
        public async Task GetFilmDetailsAsync_GetsFilm_ReturnsFilmDTO()
        {
            // Arrange
            Film film = films.First();
            mockRepo.Setup(r => r.GetFilmDetailsAsync(It.IsAny<int>())).ReturnsAsync(film);

            // Act
            var result = await service.GetFilmDetailsAsync(0);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<FilmDetailsDTO>();
            result.Should().BeEquivalentTo(FilmDetailsDTO.ToDTO(film));
        }
        // Тест на правильность передачи параметров
        [Fact]
        public async Task GetFilmDetailsAsync_GetsIdParam_PassesIdParam()
        {
            // Arrange
            int capturedId = 0;
            mockRepo.Setup(r => r.GetFilmDetailsAsync(It.IsAny<int>()))
                .Callback<int>(id => capturedId = id)
                .ReturnsAsync((Film?)null);

            // Act
            var result = await service.GetFilmDetailsAsync(7);

            // Assert
            capturedId.Should().Be(7);
        }
    }
}
