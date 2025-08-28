using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiTestApp.DTOs;
using MauiTestApp.Models.Database;
using MauiTestApp.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MauiTestApp.Services
{
    internal enum SearchBy
    {
        Name,
        Actor
    }
    // Фильтр для поиска
    internal record SearchFilter(
        string Query,
        ICollection<string> RequiredGenres,
        SearchBy SearchBy
    );

    internal class FilmsService(FilmsDBContext filmsDBContext) : IFilmsService
    {
        private FilmsDBContext dBContext = filmsDBContext;
        public async Task<ICollection<FilmEntryDTO>> SearchFilms(SearchFilter searchFilter)
        {
            ICollection<FilmEntryDTO> foundFilmsDtos;
            if (searchFilter.SearchBy == SearchBy.Name)
            {
                var foundFilms = dBContext.Films
                    .AsNoTracking()
                    .Where(f => EF.Functions.Like(f.Name.ToLower(), $"%{searchFilter.Query}%"));
                if (searchFilter.RequiredGenres.Any())
                {
                    // Для проверки, что все жанры фильтра присутствуют в фильме
                    // для каждого жанра фильма проверяется 
                    foundFilms = foundFilms
                        .Include(f => f.Genres)
                        .Where(f => searchFilter.RequiredGenres.All(rg => f.Genres.Select(g => g.GenreName).Contains(rg)));
                }
                else
                foundFilmsDtos = await foundFilms
                    .Select(f => FilmEntryDTO.ToDTO(f))
                    .ToListAsync();
            }
        }
    }
}
