using MyWeather.ViewModels;
using Xamarin.Forms;

namespace MyWeather.Views
{
    class ForecastView : ContentPage
    {
        public ForecastView()
        {
            Title = "Forecast";

            if (Device.RuntimePlatform is Device.iOS || Device.RuntimePlatform is Device.Android)
            {
                IconImageSource = "cloud";
            }

            var collectionView = new CollectionView
            {
                SelectionMode = SelectionMode.Single,
                ItemTemplate = new ForecastDataTemplate()
            };
            collectionView.SelectionChanged += HandleSelectionChanged;
            collectionView.SetBinding(CollectionView.ItemsSourceProperty, nameof(WeatherViewModel.ForecastItems));

            Content = collectionView;
        }

        void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is CollectionView collectionView)
                collectionView.SelectedItem = null;
        }
    }
}