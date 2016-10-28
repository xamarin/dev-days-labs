
using Xamarin.Forms;

using DevDaysSpeakers.Model;
using DevDaysSpeakers.ViewModel;

namespace DevDaysSpeakers.View
{
    public partial class SpeakersPage : ContentPage
    {
        SpeakersViewModel vm;
        public SpeakersPage()
        {
            InitializeComponent();

            vm = new SpeakersViewModel();

            BindingContext = vm;
            

            ListViewSpeakers.ItemSelected += ListViewSpeakers_ItemSelected;

        }

        private async void ListViewSpeakers_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var speaker = e.SelectedItem as Speaker;
            if (speaker == null)
                return;

            await Navigation.PushAsync(new DetailsPage(speaker));

            ListViewSpeakers.SelectedItem = null;

        }

       
    }
}
