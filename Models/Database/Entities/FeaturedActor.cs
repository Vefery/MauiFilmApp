using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTestApp.Models.Database.Entities
{
    // Вспомогательная сущность для реализации связи многие-ко-многим между фильмами и актерами
    public class FeaturedActor
    {
        public int ActorId { get; set; }
        public Actor? Actor { get; set; }
        public int FilmId { get; set; }
        public Film? Film { get; set; }
    }
}
