using MyWeather.ViewModels;
using Xamarin.Forms;

namespace MyWeather.Views
{
    class WeatherView : ContentPage
    {
        public WeatherView()
        {
            Title = "Weather";

            if (Device.RuntimePlatform is Device.iOS || Device.RuntimePlatform is Device.Android)
            {
                IconImageSource = "sun";
            }

            var locationEntry = new Entry
            {
                Placeholder = "City,CountryAbbreviation, e.g. Seattle,USA"
            };
            locationEntry.SetBinding(IsEnabledProperty, nameof(WeatherViewModel.IsLocationEntryEnabled));
            locationEntry.SetBinding(Entry.TextProperty, nameof(WeatherViewModel.Location));

            var useImperialLabel = new Label
            {
                Text = "Use Imperial?",
                VerticalTextAlignment = TextAlignment.Center
            };

            var useImperialSwitch = new Switch();
            useImperialSwitch.SetBinding(Switch.IsToggledProperty, nameof(WeatherViewModel.IsImperial));

            var useGPSLabel = new Label
            {
                Text = "Use GPS?",
                VerticalTextAlignment = TextAlignment.Center
            };

            var useGPSSwitch = new Switch();
            useGPSSwitch.SetBinding(Switch.IsToggledProperty, nameof(WeatherViewModel.UseGPS));

            var getWeatherButton = new Button { Text = "Get Weather" };
            getWeatherButton.SetBinding(Button.CommandProperty, nameof(WeatherViewModel.GetWeatherCommand));

            var temperatureLabel = new Label { FontSize = 30 };
            temperatureLabel.SetBinding(Label.TextProperty, nameof(WeatherViewModel.Temperature));

            var conditionLabel = new Label { Margin = new Thickness(5, 0, 0, 0) };
            conditionLabel.SetBinding(Label.TextProperty, nameof(WeatherViewModel.Condition));

            var activityIndicator = new ActivityIndicator
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            activityIndicator.SetBinding(IsVisibleProperty, nameof(WeatherViewModel.IsBusy));
            activityIndicator.SetBinding(ActivityIndicator.IsRunningProperty, nameof(WeatherViewModel.IsBusy));

            var grid = new Grid { RowSpacing = 20 };
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(50, GridUnitType.Absolute) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(100, GridUnitType.Absolute) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.Children.Add(locationEntry, 0, 0);
            Grid.SetColumnSpan(locationEntry, 3);

            grid.Children.Add(useImperialSwitch, 0, 1);
            grid.Children.Add(useImperialLabel, 1, 1);

            grid.Children.Add(useGPSSwitch, 0, 2);
            grid.Children.Add(useGPSLabel, 1, 2);

            grid.Children.Add(getWeatherButton, 0, 3);
            Grid.SetColumnSpan(getWeatherButton, 3);

            grid.Children.Add(temperatureLabel, 0, 4);
            Grid.SetColumnSpan(temperatureLabel, 3);

            grid.Children.Add(activityIndicator, 0, 4);
            Grid.SetColumnSpan(activityIndicator, 3);

            grid.Children.Add(conditionLabel, 0, 5);
            Grid.SetColumnSpan(conditionLabel, 3);

            Padding = new Thickness(10);

            Content = grid;
        }
    }
}