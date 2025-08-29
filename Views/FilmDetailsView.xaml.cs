using MauiTestApp.ViewModels;

namespace MauiTestApp.Views;

public partial class FilmDetailsView : ContentPage
{
    public FilmDetailsView(FilmDetailsViewModel filmDetailsViewModel)
	{
		InitializeComponent();
		BindingContext = filmDetailsViewModel;
	}
}