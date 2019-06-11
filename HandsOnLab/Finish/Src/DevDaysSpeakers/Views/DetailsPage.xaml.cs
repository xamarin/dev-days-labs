using System;

using DevDaysSpeakers.Shared.Models;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DevDaysSpeakers.Views
{
    public partial class DetailsPage : ContentPage
    {
        readonly Speaker speaker;

        public DetailsPage(Speaker item)
        {
            InitializeComponent();
            speaker = item;

            BindingContext = speaker;

            ButtonSpeak.Clicked += ButtonSpeak_Clicked;

            ButtonWebsite.Clicked += ButtonWebsite_Clicked;
        }

        async void ButtonWebsite_Clicked(object sender, EventArgs e)
        {
            if (speaker.Website.StartsWith("https"))
                await Browser.OpenAsync(speaker.Website);
        }

        async void ButtonSpeak_Clicked(object sender, EventArgs e)
        {
            await TextToSpeech.SpeakAsync(speaker.Description);
        }
    }
}
