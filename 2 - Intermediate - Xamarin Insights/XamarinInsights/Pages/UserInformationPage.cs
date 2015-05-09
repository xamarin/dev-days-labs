using System;
using Xamarin.Forms;

namespace XamarinInsights
{
	public class UserInformationPage : ContentPage
	{
		public UserInformationPage ()
		{
			BindingContext = new User ();
			var stack = new StackLayout { VerticalOptions = LayoutOptions.FillAndExpand };

			var name = new Entry {Placeholder = "Name"};
			name.SetBinding (Entry.TextProperty, "Name");
			stack.Children.Add (name);

			var email = new Entry {Placeholder = "Email"};
			email.SetBinding (Entry.TextProperty, "Email");
			stack.Children.Add (email);

			var photoUrl = new Entry {Placeholder = "Photo Url"};
			photoUrl.SetBinding (Entry.TextProperty, "ImageUrl");
			stack.Children.Add (photoUrl);

			var button = new Button {
				Text = "Play Game",
				Command = new Command(()=>{
					//TODO: report user
					Analytics.UserAuthenticated(BindingContext as User);
					Navigation.PushModalAsync(new LockScreen());
				})
			};
			stack.Children.Add (button);

			ScrollView scrollview = new ScrollView {
				Orientation = ScrollOrientation.Vertical,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Content = stack

			};

			this.Content = new StackLayout {
				Children = { scrollview }
			};
		}

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();
			Analytics.LogPageView ("Lock Screen");
		}
	}
}

