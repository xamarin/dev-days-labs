using MyWeather.Models;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using static Xamarin.Forms.Markup.GridRowsColumns;
using static MyWeather.MarkupExtensions;

namespace MyWeather
{
    public class ForecastDataTemplate : DataTemplate
    {
        const int imageHeight = 50;

        public ForecastDataTemplate() : base(GenerateTemplate)
        {
        }

        static Grid GenerateTemplate() => new Grid
        {
            Padding = new Thickness(10, 5, 0, 10),

            RowSpacing = 2,

            RowDefinitions = Rows.Define(
                (Row.Weather, Star),
                (Row.DateTime, Star)),

            ColumnDefinitions = Columns.Define(
                (Column.Image, AbsoluteGridLength(imageHeight + 10)),
                (Column.Forecast, Star)),

            Children =
            {
                new WeatherImage().Center()
                    .Column(Column.Image).RowSpan(All<Row>())
                    .Bind(Image.SourceProperty, nameof(WeatherRoot.DisplayIcon)),

                new TemperatureLabel().TextBottom().TextStart()
                    .Row(Row.Weather).Column(Column.Forecast)
                    .Bind(Label.TextProperty, nameof(WeatherRoot.DisplayTemp)),

                new DateTimeLabel().TextTop().TextStart()
                    .Row(Row.DateTime).Column(Column.Forecast)
                    .Bind(Label.TextProperty,nameof(WeatherRoot.DisplayDate))
            }
        };

        enum Row { Weather, DateTime }
        enum Column { Image, Forecast }

        class WeatherImage : Image
        {
            public WeatherImage()
            {
                HeightRequest = imageHeight;
                WidthRequest = imageHeight;
            }
        }

        class TemperatureLabel : Label
        {
            public TemperatureLabel()
            {
                Style = Device.Styles.ListItemTextStyle;
                TextColor = Color.FromHex("3498DB");
            }
        }

        class DateTimeLabel : Label
        {
            public DateTimeLabel() => Style = Device.Styles.ListItemDetailTextStyle;
        }
    }
}