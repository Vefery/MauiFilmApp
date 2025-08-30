using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiTestApp.Models.Database.Entities;

namespace MauiTestApp.DTOs
{
    // DTO класс для базовой информации о фильме на странице поиска
    public record FilmEntryDTO(
        int Id,
        string Name,
        string PosterPath
    )
    {
        public static FilmEntryDTO ToDTO(Film filmEntity)
        {
            return new FilmEntryDTO(
                filmEntity.Id, 
                filmEntity.Name, 
                filmEntity.PosterPath
            );
        }
    }
}
