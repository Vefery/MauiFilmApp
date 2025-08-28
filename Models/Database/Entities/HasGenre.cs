using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTestApp.Models.Database.Entities
{
    // Вспомогательная сущность для реализации связи многие-ко-многим между фильмами и жанрами
    public class HasGenre
    {
        public int FilmId { get; set; }
        public Film? Film { get; set; }
        public int GenreId { get; set; }
        public Genre? Genre { get; set; }
    }
}
