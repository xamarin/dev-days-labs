// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace TwitterSearch.iOS
{
	[Register ("RootViewController")]
	partial class RootViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIButton ButtonGetTweets { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView TableViewTweets { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField TextFieldSearch { get; set; }

		[Action ("ButtonGetTweets_TouchUpInside:")]
		[GeneratedCode ("iOS Designer", "1.0")]
		partial void ButtonGetTweets_TouchUpInside (UIButton sender);

		void ReleaseDesignerOutlets ()
		{
			if (ButtonGetTweets != null) {
				ButtonGetTweets.Dispose ();
				ButtonGetTweets = null;
			}
			if (TableViewTweets != null) {
				TableViewTweets.Dispose ();
				TableViewTweets = null;
			}
			if (TextFieldSearch != null) {
				TextFieldSearch.Dispose ();
				TextFieldSearch = null;
			}
		}
	}
}
