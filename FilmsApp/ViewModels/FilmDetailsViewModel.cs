using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MauiTestApp.DTOs;
using MauiTestApp.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace MauiTestApp.ViewModels
{
    [QueryProperty(nameof(FilmId), "FilmId")]
    public class FilmDetailsViewModel : ObservableObject
    {
        public int FilmId {
            set
            {
                LoadFilmCommand.ExecuteAsync(value);
            }
        }
        public FilmDetailsDTO? FilmDetails
        {
            get => filmDetails;
            set
            {
                filmDetails = value;
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
        public IAsyncRelayCommand<int> LoadFilmCommand { get; private set; }
        public ICommand BackCommand { get; private set; }

        private readonly IFilmsService filmsService;
        private FilmDetailsDTO? filmDetails;
        private bool isLoading = true;
        private ILogger<FilmDetailsViewModel> logger;
        public FilmDetailsViewModel(IFilmsService filmsService, ILogger<FilmDetailsViewModel> logger)
        {
            this.filmsService = filmsService;
            this.logger = logger;
            LoadFilmCommand = new AsyncRelayCommand<int>(LoadFilmDetails);
            BackCommand = new AsyncRelayCommand(GoBack);
        }
        public async Task LoadFilmDetails(int filmId)
        {
            FilmDetails = await filmsService.GetFilmDetailsAsync(filmId);
            if (FilmDetails is not null)
                IsLoading = false;
        }
        public async Task GoBack()
        {
            try
            {
                await Shell.Current.GoToAsync("//Search");
            }
            catch (Exception ex)
            {
                logger.LogError("Ошибка при возвращении: {er}", ex.Message);
            }
        }
    }
}
