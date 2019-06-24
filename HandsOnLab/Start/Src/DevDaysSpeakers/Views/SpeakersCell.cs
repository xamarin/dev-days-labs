using DevDaysSpeakers.Shared.Models;
using Xamarin.Forms;

namespace DevDaysSpeakers.Views
{
    public class SpeakersCell : ImageCell
    {
        public SpeakersCell()
        {
            this.SetBinding(TextProperty, "Name");
            this.SetBinding(DetailProperty, "Title");
            this.SetBinding(ImageSourceProperty, "Avatar");
        }
    }
}
