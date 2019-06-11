using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

using DevDaysSpeakers.Shared.Models;
using DevDaysSpeakers.ViewModel;

using Xamarin.Forms;

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
        }
    }
}
