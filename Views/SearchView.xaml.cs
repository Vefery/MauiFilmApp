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
        // ��������� �������� picker'� ������ ������������ �����
        // ��� �������������, ����� �� ��������
        searchPicker.SelectedIndex = 0;
    }
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _searchViewModel.FetchAllGenres();
    }
}