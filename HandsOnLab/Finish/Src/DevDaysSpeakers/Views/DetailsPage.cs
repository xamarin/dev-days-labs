using System;

using DevDaysSpeakers.Shared.Models;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DevDaysSpeakers.Views
{
    public class DetailsPage : ContentPage
    {
        readonly Speaker speaker;

        public DetailsPage(Speaker item)
        {
            speaker = item;

            var avatarImage = new Image
            {
                HeightRequest = 200,
                WidthRequest = 200,
                Source = speaker.Avatar
            };

            var nameLabel = new Label
            {
                FontSize = 24,
                Text = speaker.Name
            };

            var titleLabel = new Label
            {
                TextColor = Color.Purple,
                Text = speaker.Title
            };

            var descriptionLabel = new Label
            {
                Text = speaker.Description
            };

            var speakButton = new Button
            {
                Text = "Speak",
            };
            speakButton.Clicked += HandleSpeakButtonClicked;

            var websiteButton = new Button
            {
                Text = "Go to Website"
            };
            websiteButton.Clicked += HandleWebsiteButtonClicked;

            var stackLayout = new StackLayout
            {
                Spacing = 10,
                Children =
                {
                    avatarImage,
                    nameLabel,
                    titleLabel,
                    descriptionLabel,
                    speakButton,
                    websiteButton
                }
            };

            Title = speaker.Name;

            Padding = new Thickness(10);

            Content = new ScrollView { Content = stackLayout };
        }

        async void HandleWebsiteButtonClicked(object sender, EventArgs e)
        {
            if (speaker.Website.StartsWith("https"))
                await Browser.OpenAsync(speaker.Website);
        }

        async void HandleSpeakButtonClicked(object sender, EventArgs e)
        {
            await TextToSpeech.SpeakAsync(speaker.Description);
        }
    }
}
