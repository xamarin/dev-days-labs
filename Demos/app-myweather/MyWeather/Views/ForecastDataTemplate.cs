using MyWeather.Models;
using Xamarin.Forms;

namespace MyWeather
{
    public class ForecastDataTemplate : DataTemplate
    {
        public ForecastDataTemplate() : base(GenerateTemplate)
        {
        }

        static Grid GenerateTemplate()
        {
            const int imageHeight = 50;

            var weatherImage = new Image
            {
                HeightRequest = imageHeight,
                WidthRequest = imageHeight,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center
            };
            weatherImage.SetBinding(Image.SourceProperty, nameof(WeatherRoot.DisplayIcon));

            var temperatureLabel = new Label
            {
                Style = Device.Styles.ListItemTextStyle,
                TextColor = Color.FromHex("3498DB"),
                VerticalTextAlignment = TextAlignment.End
            };
            temperatureLabel.SetBinding(Label.TextProperty, nameof(WeatherRoot.DisplayTemp));

            var dateLabel = new Label
            {
                Style = Device.Styles.ListItemDetailTextStyle,
                VerticalTextAlignment = TextAlignment.Start
            };
            dateLabel.SetBinding(Label.TextProperty, nameof(WeatherRoot.DisplayDate));

            var grid = new Grid { RowSpacing = 2 };

            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            grid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });

            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(imageHeight + 10, GridUnitType.Absolute) });
            grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });

            grid.Children.Add(weatherImage, 0, 0);
            Grid.SetRowSpan(weatherImage, 2);

            grid.Children.Add(temperatureLabel, 1, 0);
            grid.Children.Add(dateLabel, 1, 1);

            grid.Padding = new Thickness(10, 5, 0, 10);

            return grid;
        }
    }
}