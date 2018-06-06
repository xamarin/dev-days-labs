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

            //Create the view model and set as binding context
            vm = new SpeakersViewModel();
            BindingContext = vm;
        }
    }
}
