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
        }
    }
}
