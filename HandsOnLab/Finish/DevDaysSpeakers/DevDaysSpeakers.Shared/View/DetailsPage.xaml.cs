using System;

using DevDaysSpeakers.Model;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DevDaysSpeakers.View
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

        void ButtonWebsite_Clicked(object sender, EventArgs e)
        {
            if (speaker.Website.StartsWith("http"))
                Device.OpenUri(new Uri(speaker.Website));
        }

        async void ButtonSpeak_Clicked(object sender, EventArgs e)
        {
            await TextToSpeech.SpeakAsync(this.speaker.Description);
        }
    }
}
