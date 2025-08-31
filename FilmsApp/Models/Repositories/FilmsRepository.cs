using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiTestApp.Models.Database;
using MauiTestApp.Models.Database.Entities;
using MauiTestApp.Models.Repositories.Interfaces;
using MauiTestApp.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace MauiTestApp.Models.Repositories
{
    public class FilmsRepository(ILogger<FilmsRepository> logger, FilmsDBContext filmsDBContext) : IFilmsRepository
    {
        private readonly ILogger logger = logger;
        private readonly FilmsDBContext dBContext = filmsDBContext;

        // Получение всех жанров из бд
        public async Task<ICollection<string>> GetAllGenresAsync()
        {
            try
            {
                return await dBContext.Genres
                    .AsNoTracking()
                    .Select(g => g.GenreName)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Ошибка при получении жанров: {er}", ex.Message);
                return [];
            }
        }
        // Получение информации о конкретном фильме
        // с присоединением жанров и актеров
        public async Task<Film?> GetFilmDetailsAsync(int filmId)
        {
            try
            {
                return await dBContext.Films
                    .AsNoTracking()
                    .Where(f => f.Id == filmId)
                    .Include(f => f.Genres)
                    .Include(f => f.Actors)
                    .FirstAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Ошибка при получении информации о фильме: {er}", ex.Message);
                return null;
            }
        }
        // Поиск по имени актера
        public async Task<ICollection<Film>> SearchFilmsByActorAsync(SearchFilter searchFilter)
        {
            try
            {
                // Поиск происходит по паттернам "запрос%" и "% запрос%",
                // т.е. фильм выбирается если запрос находится в начале
                // имени или фамилии одного из актеров. Например, при запросе
                // "райан", актер с именем "Брайан" уже не будет выбран
                var foundFilms = dBContext.Films
                    .AsNoTracking()
                    .Where(f => f.Actors.Any(a => 
                        EF.Functions.Like(a.NameNormalized, $"{searchFilter.Query}%")
                        || EF.Functions.Like(a.NameNormalized, $"% {searchFilter.Query}%")));

                if (searchFilter.RequiredGenres.Any())
                    foundFilms = FilterByGenres(foundFilms, searchFilter.RequiredGenres);

                return await foundFilms.ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Ошибка при поиске по актеру {er}", ex.Message);
                return [];
            }
        }
        // Поиск по названию фильма
        public async Task<ICollection<Film>> SearchFilmsByNameAsync(SearchFilter searchFilter)
        {
            try
            {
                // Поиск происходит по паттерну "%запрос%", т.е. фильм
                // выбирается если запрос находится в названии в любом месте
                var foundFilms = dBContext.Films
                    .AsNoTracking()
                    .Where(f => EF.Functions.Like(f.NameNormalized, $"%{searchFilter.Query}%"));
                if (searchFilter.RequiredGenres.Any())
                    foundFilms = FilterByGenres(foundFilms, searchFilter.RequiredGenres);

                return await foundFilms.ToListAsync();
            }
            catch (Exception ex)
            {
                logger.LogError("Ошибка при поиске по названию: {er}", ex.Message);
                return [];
            }
        }
        // Фильтрация по заданным жанрам
        private static IQueryable<Film> FilterByGenres(IQueryable<Film> films, ICollection<string> requiredGenres)
        {
            // для каждого фильма из списка жанров отсеиваются
            // жанры не содержащиеся в requiredGenres. Если итоговый
            // список соразмерен requiredGenres, то все нужные жанры присутствуют
            return films
                .Include(f => f.Genres)
                .Where(f => f.Genres.Count(g => requiredGenres.Contains(g.GenreName)) == requiredGenres.Count);
        }
    }
}
