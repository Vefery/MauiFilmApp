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

namespace MauiTestApp.ViewModels
{
    public enum SearchBy
    {
        Name,
        Actor
    }
    public class PickerItem
    {
        public SearchBy Type { get; set; }
        // Псевдоним для пользователя
        public required string DisplayName { get; set; }
    }
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

        // Простоая ObservableCollection не сработает, т.к. не реагирует на переприсвоение
        public ICollection<FilmEntryDTO> SearchResults
        {
            get => searchResults;
            set
            {
                searchResults = value; 
                OnPropertyChanged();
            }
        }
        public ObservableCollection<string> RequiredGenres { get; private set; } = [];
        public ICollection<PickerItem> SearchPickerItems { get; private set; }
        public PickerItem CurrentSearchOption
        {
            get => currentSearchOption;
            set
            {
                currentSearchOption = value;
                OnPropertyChanged();
            }
        }

        private string searchQuery = string.Empty;
        private readonly IFilmsService filmsService;
        private PickerItem currentSearchOption;
        private ICollection<FilmEntryDTO> searchResults = [];

        public SearchViewModel(IFilmsService filmsService)
        {
            SearchCommand = new AsyncRelayCommand(SearchFilmsAsync);
            this.filmsService = filmsService;
            SearchPickerItems = [
                new PickerItem { DisplayName = "Названию", Type = SearchBy.Name},
                new PickerItem { DisplayName = "Имени актера", Type = SearchBy.Actor}
            ];
            currentSearchOption = SearchPickerItems.First();
        }
        private async Task SearchFilmsAsync()
        {
            SearchFilter filter = new(SearchQuery.ToUpper(), RequiredGenres);

            if (CurrentSearchOption.Type == SearchBy.Name)
                SearchResults = new ObservableCollection<FilmEntryDTO>(await filmsService.SearchFilmsByName(filter));
            else if (CurrentSearchOption.Type == SearchBy.Actor)
                SearchResults = new ObservableCollection<FilmEntryDTO>(await filmsService.SearchFilmsByActor(filter));
        }
    }
}
