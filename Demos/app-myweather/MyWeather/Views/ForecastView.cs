using System;
using MyWeather.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Markup;

namespace MyWeather.Views
{
    class ForecastView : ContentPage
    {
        public ForecastView()
        {
            Title = "Forecast";

            IconImageSource = Device.RuntimePlatform switch
            {
                Device.iOS => "cloud",
                Device.Android => "cloud",
                Device.UWP => null,
                _ => throw new NotSupportedException()
            };

            Content = new CollectionView
            {
                SelectionMode = SelectionMode.None,
                ItemTemplate = new ForecastDataTemplate()
            }.Assign(out CollectionView collectionView)
             .Bind(CollectionView.ItemsSourceProperty, nameof(WeatherViewModel.ForecastItems));

            collectionView.SelectionChanged += HandleSelectionChanged;
        }

        void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var collectionView = (CollectionView)sender;
            collectionView.SelectedItem = null;
        }
    }
}