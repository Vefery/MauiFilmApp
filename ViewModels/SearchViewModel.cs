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
using MauiTestApp.Services.Interfaces;
using MauiTestApp.ViewModels.Misc;

namespace MauiTestApp.ViewModels
{
    public class SearchViewModel : ObservableObject
    {
        public string SearchQuery {
            get => searchQuery;
            set {
                searchQuery = value;
                OnPropertyChanged();
            }
        }
        public SearchBy SearchBy
        {
            get => CurrentSearchOption.Type;
        }
        public ICommand SearchCommand { get; private set; }
        public ICommand ToggleFiltersPanel {  get; private set; }

        // Простоая ObservableCollection не сработает, т.к. не реагирует на переприсвоение
        public IEnumerable<FilmEntryDTO> SearchResults
        {
            get => searchResults;
            set
            {
                searchResults = value; 
                OnPropertyChanged();
            }
        }
        public ICollection<SearchGenreSelection> GenresFilter
        {
            get => genresFilter;
            set
            {
                genresFilter = value;
                OnPropertyChanged();
            }
        }
        public ICollection<SearchPickerItem> SearchPickerItems { get; private set; }
        public SearchPickerItem CurrentSearchOption
        {
            get => currentSearchOption;
            set
            {
                currentSearchOption = value;
                OnPropertyChanged();
            }
        }
        public bool IsFilterExpanded
        {
            get => isFilterExpanded;
            set
            {
                isFilterExpanded = value;
                OnPropertyChanged();
            }
        }

        private string searchQuery = string.Empty;
        private readonly IFilmsService filmsService;
        private bool isFilterExpanded;
        private SearchPickerItem currentSearchOption;
        private IEnumerable<FilmEntryDTO> searchResults = [];
        private ICollection<SearchGenreSelection> genresFilter = [];

        public SearchViewModel(IFilmsService filmsService)
        {
            SearchCommand = new AsyncRelayCommand(SearchFilmsAsync);
            ToggleFiltersPanel = new Command(() => IsFilterExpanded = !IsFilterExpanded);
            this.filmsService = filmsService;
            SearchPickerItems = [
                new SearchPickerItem { DisplayName = "Названию", Type = SearchBy.Name},
                new SearchPickerItem { DisplayName = "Имени актера", Type = SearchBy.Actor}
            ];
            currentSearchOption = SearchPickerItems.First();
        }
        public async Task FetchAllGenres()
        {
            GenresFilter = (await filmsService.GetAllGenres()).Select(g => new SearchGenreSelection(g)).ToList();
        }
        private async Task SearchFilmsAsync()
        {
            SearchFilter filter = new(SearchQuery.ToUpper(), GenresFilter.Where(g => g.IsSelected).Select(g => g.Name));

            if (CurrentSearchOption.Type == SearchBy.Name)
                SearchResults = new ObservableCollection<FilmEntryDTO>(await filmsService.SearchFilmsByName(filter));
            else if (CurrentSearchOption.Type == SearchBy.Actor)
                SearchResults = new ObservableCollection<FilmEntryDTO>(await filmsService.SearchFilmsByActor(filter));
        }
    }
}
