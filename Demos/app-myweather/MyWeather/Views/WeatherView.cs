using System;
using MyWeather.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using static Xamarin.Forms.Markup.GridRowsColumns;
using static MyWeather.MarkupExtensions;

namespace MyWeather.Views
{
    class WeatherView : ContentPage
    {
        public WeatherView()
        {
            Padding = new Thickness(10);

            Title = "Weather";

            IconImageSource = Device.RuntimePlatform switch
            {
                Device.iOS => "sun",
                Device.Android => "sun",
                Device.UWP => null,
                _ => throw new NotSupportedException()
            };

            const int temperatureFontSize = 30;

            Content = new Grid
            {
                RowSpacing = 20,

                RowDefinitions = Rows.Define(
                    (Row.LocationInput, AbsoluteGridLength(50)),
                    (Row.ImperialSwitch, AbsoluteGridLength(30)),
                    (Row.GpsSwitch, AbsoluteGridLength(30)),
                    (Row.GetWeatherButton, AbsoluteGridLength(40)),
                    (Row.TemperatureForecast, AbsoluteGridLength(temperatureFontSize + 10)),
                    (Row.ConditionsForecast, Star)),

                ColumnDefinitions = Columns.Define(
                    (Column.Toggle, AbsoluteGridLength(50)),
                    (Column.Description, Star)),

                Children =
                {
                    new Entry { Placeholder = "City,CountryAbbreviation, e.g. Seattle,USA" }
                        .Row(Row.LocationInput).ColumnSpan(All<Column>())
                        .Bind<Entry, bool, bool>(IsEnabledProperty, nameof(WeatherViewModel.UseGPS), convert: useGps => !useGps)
                        .Bind(Entry.TextProperty, nameof(WeatherViewModel.Location)),

                    new Label { Text = "Use Imperial?" }.TextCenterVertical()
                        .Row(Row.ImperialSwitch).Column(Column.Description),

                    new Switch()
                        .Row(Row.ImperialSwitch).Column(Column.Toggle)
                        .Bind(Switch.IsToggledProperty, nameof(WeatherViewModel.IsImperial)),

                    new Label { Text = "Use GPS?" }
                        .Row(Row.GpsSwitch).Column(Column.Description),

                    new Switch()
                        .Row(Row.GpsSwitch).Column(Column.Toggle)
                        .Bind(Switch.IsToggledProperty, nameof(WeatherViewModel.UseGPS)),

                    new Button { Text = "Get Weather" }
                        .Row(Row.GetWeatherButton).ColumnSpan(All<Column>())
                        .Bind(Button.CommandProperty, nameof(WeatherViewModel.GetWeatherCommand)),

                    new Label { FontSize = temperatureFontSize }.TextTop().TextStart()
                        .Row(Row.TemperatureForecast).ColumnSpan(All<Column>())
                        .Bind(Label.TextProperty, nameof(WeatherViewModel.Temperature)),

                    new ActivityIndicator().Center()
                        .Row(Row.TemperatureForecast).ColumnSpan(All<Column>())
                        .Bind(IsVisibleProperty, nameof(WeatherViewModel.IsBusy))
                        .Bind(ActivityIndicator.IsRunningProperty, nameof(WeatherViewModel.IsBusy)),

                    new Label().Margins(5, 0, 0, 0).TextTop().TextStart()
                        .Row(Row.ConditionsForecast).ColumnSpan(All<Column>())
                        .Bind(Label.TextProperty, nameof(WeatherViewModel.Condition)),
                }
            };

        }

        enum Row { LocationInput, ImperialSwitch, GpsSwitch, GetWeatherButton, TemperatureForecast, ConditionsForecast }
        enum Column { Toggle, Description }
    }
}