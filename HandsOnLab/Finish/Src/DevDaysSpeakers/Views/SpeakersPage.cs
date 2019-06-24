using System.Collections.ObjectModel;
using DevDaysSpeakers.Shared.Models;
using DevDaysSpeakers.ViewModel;
using Xamarin.Forms;

namespace DevDaysSpeakers.Views
{
    public class SpeakersPage : ContentPage
    {
        readonly ListView speakersListView;

        public SpeakersPage()
        {
            BindingContext = new SpeakersViewModel();

            speakersListView = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                IsPullToRefreshEnabled = true,
                ItemTemplate = new DataTemplate(typeof(SpeakersCell)),
                SeparatorColor = Color.Transparent
            };
            speakersListView.SetBinding(ListView.ItemsSourceProperty, nameof(SpeakersViewModel.Speakers));
            speakersListView.SetBinding(ListView.RefreshCommandProperty, nameof(SpeakersViewModel.GetSpeakersCommand));
            speakersListView.SetBinding(ListView.IsRefreshingProperty, nameof(SpeakersViewModel.IsBusy));
            speakersListView.ItemSelected += ListViewSpeakers_ItemSelected;

            Title = "Speakers";

            Content = speakersListView;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (speakersListView.ItemsSource is ObservableCollection<Speaker> speakerCollection && speakerCollection.Count is 0)
            {
                speakersListView.BeginRefresh();
            }
        }

        async void ListViewSpeakers_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (sender is ListView listView && e.SelectedItem is Speaker speaker)
            {
                listView.SelectedItem = null;
                await Navigation.PushAsync(new DetailsPage(speaker));
            }
        }
    }
}
