using Xamarin.Forms;

using DevDaysSpeakers.Shared.Models;
using DevDaysSpeakers.ViewModel;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace DevDaysSpeakers.Views
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

            if (ListViewSpeakers.ItemsSource is ObservableCollection<Speaker> speakerCollection && speakerCollection.Count is 0)
            {
                ListViewSpeakers.BeginRefresh();
            }
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
