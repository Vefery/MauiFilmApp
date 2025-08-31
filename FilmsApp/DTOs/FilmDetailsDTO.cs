using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MauiTestApp.Models.Database.Entities;

namespace MauiTestApp.DTOs
{
    // DTO класс для детальной информации о фильме
    public record FilmDetailsDTO(
        string Name,
        string Description,
        string PosterPath,
        string ReleaseDate,
        ICollection<string> Genres,
        ICollection<string> Actors
    )
    {
        public static FilmDetailsDTO ToDTO(Film filmEntity)
        {
            return new FilmDetailsDTO(
                filmEntity.Name,
                filmEntity.Description,
                filmEntity.PosterPath,
                filmEntity.ReleaseDate.ToString("d MMMM yyyy"),
                [.. filmEntity.Genres.Select(g => g.GenreName)],
                [.. filmEntity.Actors.Select(a => a.Name)]
            );
        }
    }
}
