using MauiTestApp.ViewModels;

namespace MauiTestApp.Views;

public partial class SearchView : ContentPage
{
	public SearchView(SearchViewModel searchViewModel)
	{
		InitializeComponent();
		BindingContext = searchViewModel;
	}
    protected override void OnHandlerChanged()
    {
        base.OnHandlerChanged();
        // Начальное значение picker'а должно выставляться после
        // его инициализации, иначе не работает
        searchPicker.SelectedIndex = 0;
    }
}