using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiTestApp.DTOs;

namespace MauiTestApp.Services.Interfaces
{
    public interface IFilmsService
    {
        Task<ICollection<FilmEntryDTO>> SearchFilmsByName(SearchFilter searchFilter);
        Task<ICollection<FilmEntryDTO>> SearchFilmsByActor(SearchFilter searchFilter);
    }
}
