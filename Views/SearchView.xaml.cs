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
        // Начальное значение picker'а должно выставляться после
        // его инициализации, иначе не работает
        searchPicker.SelectedIndex = 0;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _searchViewModel.FetchAllGenres();
    }
}