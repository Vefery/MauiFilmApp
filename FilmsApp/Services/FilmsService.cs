using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiTestApp.DTOs;
using MauiTestApp.Models.Database;
using MauiTestApp.Models.Database.Entities;
using MauiTestApp.Models.Repositories.Interfaces;
using MauiTestApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MauiTestApp.Services
{
    // Фильтр для поиска
    public record SearchFilter(
        string Query,
        IEnumerable<string> RequiredGenres
    );

    public class FilmsService(IFilmsRepository repository, ILogger<FilmsService> logger) : IFilmsService
    {
        private readonly IFilmsRepository repository = repository;
        private readonly ILogger<FilmsService> logger = logger;

        // Поиск по имени актера
        public async Task<ICollection<FilmEntryDTO>> SearchFilmsByActorAsync(SearchFilter searchFilter)
        {
            return (await repository.SearchFilmsByActorAsync(searchFilter))
                .Select(f => FilmEntryDTO.ToDTO(f))
                .ToList();
        }

        // Поиск по имени фильма
        public async Task<ICollection<FilmEntryDTO>> SearchFilmsByNameAsync(SearchFilter searchFilter)
        {
            return (await repository.SearchFilmsByNameAsync(searchFilter))
                .Select(f => FilmEntryDTO.ToDTO(f))
                .ToList();
        }

        // Получение списка всех жанров
        public async Task<ICollection<string>> GetAllGenresAsync() => await repository.GetAllGenresAsync();
        public async Task<FilmDetailsDTO?> GetFilmDetailsAsync(int filmId)
        {
            Film? resultFilm = await repository.GetFilmDetailsAsync(filmId);
            return resultFilm is not null ? FilmDetailsDTO.ToDTO(resultFilm) : null;
        }
    }
}
