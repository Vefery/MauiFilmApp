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
        // ��������� �������� picker'� ������ ������������ �����
        // ��� �������������, ����� �� ��������
        searchPicker.SelectedIndex = 0;
    }
}