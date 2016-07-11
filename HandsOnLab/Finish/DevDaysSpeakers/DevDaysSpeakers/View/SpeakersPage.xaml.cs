using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;

using DevDaysSpeakers.Model;
using DevDaysSpeakers.ViewModel;
using AppServiceHelpers;
using AppServiceHelpers.Abstractions;

namespace DevDaysSpeakers.View
{
    public partial class SpeakersPage : ContentPage
    {
        SpeakersViewModel vm;
        public SpeakersPage(IEasyMobileServiceClient client)
        {
            InitializeComponent();

            vm = new SpeakersViewModel(client);

            BindingContext = vm;

            ButtonSpeakers.Clicked += ButtonSpeakers_Clicked;

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

        private async void ButtonSpeakers_Clicked(object sender, EventArgs e)
        {
            Exception ex = null;

            try
            {
                ButtonSpeakers.IsEnabled = false;

                await vm.GetSpeakers();
            }
            catch(Exception error)
            {
                ex = error;
            }
            finally
            {
                ButtonSpeakers.IsEnabled = true;
            }

            if (ex != null)
                await DisplayAlert("Error!", ex.Message, "OK");
        }
    }
}
