using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTestApp.Models.Database.Entities
{
    // Сущность "Фильм" для базы данных. В качестве жанра ссылка на сущность из таблицы Genres
    public class Film
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        // SQLite не работает с регистром кириллицы, поэтому для поиска нормализованное поле
        public required string NameNormalized { get; set; }
        public required string Description { get; set; }
        public DateOnly ReleaseDate {  get; set; }
        public ICollection<Genre> Genres { get; set; } = [];
        public ICollection<Actor> Actors { get; } = [];
    }
}
