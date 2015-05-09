using System;
using System.Linq;
using Xamarin.Forms;

namespace XamarinInsights
{
	public class LockScreen : ContentPage
	{
		Level level;

		public Level Level {
			get { return level; }
			set { BindingContext = level = value; }
		}

		public LockScreen ()
		{
			Level = new Level ();
			StackLayout stack = new StackLayout ();
			stack.VerticalOptions = LayoutOptions.FillAndExpand;

			var box = new BoxView{ HeightRequest = 40 };
			stack.Children.Add (box);

			var levelLabel = new Label {
				XAlign = TextAlignment.Center,
				Font = Font.SystemFontOfSize (NamedSize.Medium),
			};
			levelLabel.SetBinding (Label.TextProperty, "CurrentLevel");
			stack.Children.Add (levelLabel);

			var timeRemaining = new Label {
				Font = Font.SystemFontOfSize (NamedSize.Large),
				TextColor = Color.Red,
			};
			timeRemaining.SetBinding (Label.TextProperty, "TimeRemainingDisplay");
			stack.Children.Add (timeRemaining);

			stack.Children.Add (new Label {
				Text = "Passcode",
				XAlign = TextAlignment.Center,
				Font = Font.SystemFontOfSize (NamedSize.Large)
			});

			var passCode = new Label {
				XAlign = TextAlignment.Center,
				Font = Font.SystemFontOfSize (NamedSize.Medium),
				TextColor = Color.Gray
			};
			passCode.SetBinding (Label.TextProperty, "CurrentCombination");
			stack.Children.Add (passCode);

			var currentPass = new Label {
				XAlign = TextAlignment.Center,
				YAlign = TextAlignment.Center,
				Font = Font.SystemFontOfSize (NamedSize.Large),
				HeightRequest = 50,
			};
			currentPass.SetBinding (Label.TextProperty, "CombinationSoFar");
			stack.Children.Add (currentPass);

			var grid = new Grid () {
				VerticalOptions = new LayoutOptions (LayoutAlignment.Fill, true),
				HorizontalOptions = new LayoutOptions (LayoutAlignment.Fill, true),
			};
			Enumerable.Range (0, 3).ToList ().ForEach (x => grid.ColumnDefinitions.Add (new ColumnDefinition { Width = new GridLength (1, GridUnitType.Star) }));

			Enumerable.Range (0, 4).ToList ().ForEach (x => grid.RowDefinitions.Add (new RowDefinition { Height = new GridLength (1, GridUnitType.Star) }));
			stack.Children.Add (grid);

			grid.Children.Add (CreateKey ("1"), 0, 0);
			grid.Children.Add (CreateKey ("2"), 1, 0);
			grid.Children.Add (CreateKey ("3"), 2, 0);
			grid.Children.Add (CreateKey ("4"), 0, 1);
			grid.Children.Add (CreateKey ("5"), 1, 1);
			grid.Children.Add (CreateKey ("6"), 2, 1);
			grid.Children.Add (CreateKey ("7"), 0, 2);
			grid.Children.Add (CreateKey ("8"), 1, 2);
			grid.Children.Add (CreateKey ("9"), 2, 2);
			grid.Children.Add (CreateKey ("0"), 1, 3);

			Content = stack;
		}

		Button CreateKey (string key)
		{
			var button = new Button {
				Text = key,
				Font = Font.SystemFontOfSize (NamedSize.Large),
				Command = new Command (() => Level.PressKey (key)),
				VerticalOptions = new LayoutOptions (LayoutAlignment.Fill, true),
				HorizontalOptions = new LayoutOptions (LayoutAlignment.Fill, true),
			};
			return button;
		}

		protected override async void OnAppearing ()
		{
			base.OnAppearing ();
			Analytics.LogPageView ("Lock Screen");
			level.NewLevel ();
			level.StopTimer ();
			await DisplayAlert ("Hurry!", string.Format ("You have {0} seconds to type the passcode", Level.TimeRemaining / 1000), "Go");
			level.StartTimer ();
		}
	}
}

