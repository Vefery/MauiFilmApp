using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentAssertions;
using MauiTestApp.DTOs;
using MauiTestApp.Models.Database.Entities;

namespace FilmsApp.Tests.DTOTests
{
    public class FilmDtoTests
    {
        public Film filmEntity;
        public FilmDtoTests()
        {
            filmEntity = new()
            {
                Id = 1,
                Name = "Фильм",
                NameNormalized = "ФИЛЬМ",
                Description = "Описание фильма",
                Actors = [new Actor() { Id = 1, Name = "Имя актера", NameNormalized = "ИМЯ АКТЕРА" }],
                Genres = [new Genre() { Id = 1, GenreName = "Драма" }, new Genre() { Id = 2, GenreName = "История" }],
                PosterPath = "poster.png",
                ReleaseDate = new DateOnly(2025, 1, 1)
            };
        }

        [Fact]
        public void ToDTO_ConvertEntityToDetailsDTO_ReturnsDetailsDTO()
        {
            // Act
            var filmDto = FilmDetailsDTO.ToDTO(filmEntity);

            // Assert
            filmDto.Should().BeOfType<FilmDetailsDTO>();
            filmDto.Name.Should().Be(filmEntity.Name);
            filmDto.Description.Should().Be(filmEntity.Description);
            filmDto.PosterPath.Should().Be(filmEntity.PosterPath);
            filmDto.ReleaseDate.Should().Be(filmEntity.ReleaseDate.ToString("d MMMM yyyy"));
            filmDto.Genres.Should().OnlyHaveUniqueItems();
            filmDto.Genres.Should().BeEquivalentTo(filmEntity.Genres.Select(g => g.GenreName));
            filmDto.Actors.Should().OnlyHaveUniqueItems();
            filmDto.Actors.Should().BeEquivalentTo(filmEntity.Actors.Select(a => a.Name));
        }
        [Fact]
        public void ToDTO_ConvertEntityToEntryDTO_ReturnsEntryDTO()
        {
            // Act
            var filmDto = FilmEntryDTO.ToDTO(filmEntity);

            // Assert
            filmDto.Should().BeOfType<FilmEntryDTO>();
            filmDto.Id.Should().Be(filmEntity.Id);
            filmDto.Name.Should().Be(filmEntity.Name);
            filmDto.PosterPath.Should().Be(filmEntity.PosterPath);
        }
    }
}
