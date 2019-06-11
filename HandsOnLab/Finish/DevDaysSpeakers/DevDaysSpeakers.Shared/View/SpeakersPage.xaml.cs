using Xamarin.Forms;

using DevDaysSpeakers.Model;
using DevDaysSpeakers.ViewModel;

namespace DevDaysSpeakers.View
{
    public partial class SpeakersPage : ContentPage
    {
        readonly SpeakersViewModel vm;

        public SpeakersPage()
        {
            InitializeComponent();

            vm = new SpeakersViewModel();

            BindingContext = vm;

            ListViewSpeakers.ItemSelected += ListViewSpeakers_ItemSelected;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ListViewSpeakers.BeginRefresh();
        }

        async void ListViewSpeakers_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem is Speaker speaker)
            {
                await Navigation.PushAsync(new DetailsPage(speaker));
                ListViewSpeakers.SelectedItem = null;
            }
        }
    }
}
