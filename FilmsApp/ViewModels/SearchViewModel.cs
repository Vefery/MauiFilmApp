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
using Microsoft.Extensions.Logging;

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
        public IAsyncRelayCommand SearchCommand { get; private set; }
        public ICommand ToggleFiltersPanelCommand {  get; private set; }
        public IAsyncRelayCommand ShowFilmDetailsCommand {  get; private set; }

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
        public bool IsLoading
        {
            get => isLoading;
            set
            {
                isLoading = value;
                OnPropertyChanged();
            }
        }

        private string searchQuery = string.Empty;
        private readonly IFilmsService filmsService;
        private bool isFilterExpanded;
        private bool isLoading;
        private SearchPickerItem currentSearchOption;
        private IEnumerable<FilmEntryDTO> searchResults = [];
        private ICollection<SearchGenreSelection> genresFilter = [];
        private ILogger<SearchViewModel> logger;
        private bool isInitialized;

        public SearchViewModel(IFilmsService filmsService, ILogger<SearchViewModel> logger)
        {
            SearchCommand = new AsyncRelayCommand(SearchFilmsAsync);
            ShowFilmDetailsCommand = new AsyncRelayCommand<int>(NavigateToFilm);
            ToggleFiltersPanelCommand = new Command(() => IsFilterExpanded = !IsFilterExpanded);
            this.filmsService = filmsService;
            this.logger = logger;
            SearchPickerItems = [
                new SearchPickerItem { DisplayName = "Названию", Type = SearchBy.Name},
                new SearchPickerItem { DisplayName = "Имени актера", Type = SearchBy.Actor}
            ];
            currentSearchOption = SearchPickerItems.First();
        }
        // Первичная инициализация фильмов
        public async Task InotializeFilms()
        {
            if (!isInitialized)
            {
                await FetchAllGenresAsync();
                await FetchFilmsAsync();
                isInitialized = true;
            }
        }
        // Получение всех жанров
        private async Task FetchAllGenresAsync()
        {
            GenresFilter = (await filmsService.GetAllGenresAsync()).Select(g => new SearchGenreSelection(g)).ToList();
        }
        // Первоначальная подгрузка всех фильмов
        private async Task FetchFilmsAsync()
        {
            IsLoading = true;
            SearchFilter filter = new(string.Empty, []);
            SearchResults = new ObservableCollection<FilmEntryDTO>(await filmsService.SearchFilmsByNameAsync(filter));
            IsLoading = false;
        }
        // Навигация на страницу выбранного фильма
        private async Task NavigateToFilm(int filmId)
        {
            try
            {
                await Shell.Current.GoToAsync($"//FilmDetails?FilmId={filmId}");
            }
            catch (Exception ex)
            {
                logger.LogError("Ошибка при переходе на фильм: {er}", ex.Message);
            }
        }
        // Поиск фильмов с учетом фильтров
        private async Task SearchFilmsAsync()
        {
            IsLoading = true;
            SearchFilter filter = new(SearchQuery.ToUpper(), [.. GenresFilter.Where(g => g.IsSelected).Select(g => g.Name)]);

            if (CurrentSearchOption.Type == SearchBy.Name)
                SearchResults = new ObservableCollection<FilmEntryDTO>(await filmsService.SearchFilmsByNameAsync(filter));
            else if (CurrentSearchOption.Type == SearchBy.Actor)
                SearchResults = new ObservableCollection<FilmEntryDTO>(await filmsService.SearchFilmsByActorAsync(filter));

            IsLoading = false;
        }
    }
}
