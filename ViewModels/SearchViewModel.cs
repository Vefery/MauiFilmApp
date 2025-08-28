using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiTestApp.DTOs;
using MauiTestApp.Services;

namespace MauiTestApp.ViewModels
{
    internal class SearchViewModel : ObservableObject
    {
        public string SearchQuery {
            get => searchQuery;
            set {
                searchQuery = value.ToLower();
                OnPropertyChanged();
            }
        }
        public SearchBy SearchBy
        {
            get => searchBy;
            set
            {
                searchBy = value;
                OnPropertyChanged();
            }
        }
        public ICommand SearchCommand { get; private set; }
        public ObservableCollection<FilmEntryDTO> SearchResults { get; private set; } = [];
        public ObservableCollection<string> RequiredGenres { get; private set; } = [];

        private string searchQuery = string.Empty;
        private SearchBy searchBy = SearchBy.Name;

        public SearchViewModel()
        {
            SearchCommand = new AsyncRelayCommand(SearchFilmsAsync);
        }
        private async Task SearchFilmsAsync()
        {
            // Построение фильтра поиска
            SearchFilter filter = new(SearchQuery, RequiredGenres, SearchBy);
        }
    }
}
