using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiTestApp.DTOs;

namespace MauiTestApp.Services.Interfaces
{
    internal interface IFilmsService
    {
        Task<ICollection<FilmEntryDTO>> SearchFilms(SearchFilter searchFilter);
    }
}
