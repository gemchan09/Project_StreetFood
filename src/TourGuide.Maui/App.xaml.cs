using TouristGuide.Maui.Services;
using TouristGuide.Maui.Views;

namespace TouristGuide.Maui;

public partial class App : Application
{
	private readonly ApiService _api;

	public App(ApiService api)
	{
		InitializeComponent();
		_api = api;
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		// Always show tour selection first
		return new Window(new TourSelectionPage(_api));
	}
}