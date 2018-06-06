using System;

using DevDaysSpeakers.Model;

using Xamarin.Essentials;
using Xamarin.Forms;

namespace DevDaysSpeakers.View
{
    public partial class DetailsPage : ContentPage
    {
        readonly Speaker speaker;

        public DetailsPage(Speaker speaker)
        {
            InitializeComponent();
            
            //Set local instance of speaker and set BindingContext
            this.speaker = speaker;
            BindingContext = this.speaker;
        }
    }
}
