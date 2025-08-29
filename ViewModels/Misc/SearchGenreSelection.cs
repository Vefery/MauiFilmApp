using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MauiTestApp.ViewModels.Misc
{
    public class SearchGenreSelection
    {
        public bool IsSelected { get; set; }
        public string Name { get; set; }

        public SearchGenreSelection(string name)
        {
            Name = name;
        }
    }
}
