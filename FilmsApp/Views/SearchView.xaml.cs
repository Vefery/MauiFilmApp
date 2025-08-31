using MauiTestApp.ViewModels;

namespace MauiTestApp.Views;

public partial class SearchView : ContentPage
{
    private readonly SearchViewModel _searchViewModel;
    public SearchView(SearchViewModel searchViewModel)
	{
		InitializeComponent();
		BindingContext = _searchViewModel = searchViewModel;
	}
    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        // Ќачальное значение picker'а должно выставл€тьс€ после
        // его инициализации, иначе не работает
        searchPicker.SelectedIndex = 0;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        // ѕервоначальна€ подгрузка
        await _searchViewModel.InotializeFilms();
    }
}