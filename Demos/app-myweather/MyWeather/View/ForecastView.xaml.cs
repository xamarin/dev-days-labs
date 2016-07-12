using Xamarin.Forms;

namespace MyWeather.View
{
    public partial class ForecastView : ContentPage
    {
        public ForecastView()
        {
            InitializeComponent();
            if (Device.OS == TargetPlatform.iOS)
                Icon = new FileImageSource { File = "tab2.png" };
            ListViewWeather.ItemTapped += (sender, args) => ListViewWeather.SelectedItem = null;
        }
    }
}
