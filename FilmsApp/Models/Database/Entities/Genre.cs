using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTestApp.Models.Database.Entities
{
    // Сущность "Жанр" для базы данных
    public class Genre
    {
        public int Id { get; set; }
        public required string GenreName { get; set; }
        public ICollection<Film>? Films { get; set; }
    }
}
