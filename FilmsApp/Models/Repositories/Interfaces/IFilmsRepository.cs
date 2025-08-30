using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiTestApp.Models.Database.Entities;
using MauiTestApp.Services;

namespace MauiTestApp.Models.Repositories.Interfaces
{
    public interface IFilmsRepository
    {
        Task<ICollection<Film>> SearchFilmsByActorAsync(SearchFilter searchFilter);
        Task<ICollection<Film>> SearchFilmsByNameAsync(SearchFilter searchFilter);
        Task<ICollection<string>> GetAllGenresAsync();
        Task<Film?> GetFilmDetailsAsync(int filmId);
    }
}
