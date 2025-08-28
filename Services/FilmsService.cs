using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiTestApp.DTOs;
using MauiTestApp.Models.Database;
using MauiTestApp.Models.Database.Entities;
using MauiTestApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MauiTestApp.Services
{
    // Фильтр для поиска
    public record SearchFilter(
        string Query,
        ICollection<string> RequiredGenres
    );

    internal class FilmsService(FilmsDBContext filmsDBContext, ILogger<FilmsService> logger) : IFilmsService
    {
        private readonly FilmsDBContext dBContext = filmsDBContext;
        private readonly ILogger<FilmsService> logger = logger;

        // Поиск по имени актера
        public async Task<ICollection<FilmEntryDTO>> SearchFilmsByActor(SearchFilter searchFilter)
        {
            try
            {
                var foundFilms = dBContext.Films
                    .AsNoTracking()
                    .Where(f => f.Actors.Any(a => EF.Functions.Like(a.NameNormalized, $"%{searchFilter.Query}%")));

                if (searchFilter.RequiredGenres.Any())
                    foundFilms = FilterByGenres(foundFilms, searchFilter.RequiredGenres);

                return await foundFilms
                    .Select(f => FilmEntryDTO.ToDTO(f))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                return [];
            }
        }

        // Поиск по имени фильма
        public async Task<ICollection<FilmEntryDTO>> SearchFilmsByName(SearchFilter searchFilter)
        {
            try
            {
                var foundFilms = dBContext.Films
                    .AsNoTracking()
                    .Where(f => EF.Functions.Like(f.NameNormalized, $"%{searchFilter.Query}%"));
                if (searchFilter.RequiredGenres.Any())
                    foundFilms = FilterByGenres(foundFilms, searchFilter.RequiredGenres);
                var tmp = foundFilms
                    .Select(f => FilmEntryDTO.ToDTO(f))
                    .ToList();


                return await foundFilms
                    .Select(f => FilmEntryDTO.ToDTO(f))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);

                return [];
            }
        }

        private static IQueryable<Film> FilterByGenres(IQueryable<Film> films, ICollection<string> genres)
        {
            // Для проверки, что все жанры фильтра присутствуют в фильме
            // для каждого жанра фильма проверяется 
            return films
                    .Include(f => f.Genres)
                    .Where(f => genres.All(rg => f.Genres.Select(g => g.GenreName).Contains(rg)));
        }
    }
}
