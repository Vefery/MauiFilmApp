using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTestApp.Models.Database.Entities
{
    // Сущность "Актер" для базы данных
    public class Actor
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Surname { get; set; }
        public ICollection<Film> Films { get; } = [];
    }
}
