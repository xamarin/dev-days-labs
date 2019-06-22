using Xamarin.Forms;

namespace MyWeather.View
{
    public partial class ForecastView : ContentPage
    {
        public ForecastView()
        {
            InitializeComponent();

            if (Device.RuntimePlatform.Equals(Device.iOS))
                IconImageSource = "tab2.png";
        }

        void OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            if (sender is ListView listView)
                listView.SelectedItem = null;
        }
    }
}
