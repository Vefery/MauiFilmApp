using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTestApp.ViewModels.Misc
{
    public enum SearchBy
    {
        Name,
        Actor
    }
    // Всопмогательный класс для выбора режима поиска
    public class SearchPickerItem
    {
        public SearchBy Type { get; set; }
        // Псевдоним для пользователя
        public required string DisplayName { get; set; }
    }
}
