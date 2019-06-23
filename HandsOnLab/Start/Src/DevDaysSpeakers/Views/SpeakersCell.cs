using DevDaysSpeakers.Shared.Models;
using Xamarin.Forms;

namespace DevDaysSpeakers.Views
{
    public class SpeakersCell : ImageCell
    {
        public SpeakersCell()
        {
            this.SetBinding(TextProperty, nameof(Speaker.Name));
            this.SetBinding(DetailProperty, nameof(Speaker.Title));
            this.SetBinding(ImageSourceProperty, nameof(Speaker.Avatar));
        }
    }
}
