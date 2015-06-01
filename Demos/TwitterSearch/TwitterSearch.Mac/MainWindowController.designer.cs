// WARNING
//
// This file has been generated automatically by Xamarin Studio to store outlets and
// actions made in the UI designer. If it is removed, they will be lost.
// Manual changes to this file may not be handled correctly.
//
using Foundation;
using System.CodeDom.Compiler;

namespace TwitterSearch.Mac
{
	[Register ("MainWindowController")]
	partial class MainWindowController
	{
		[Outlet]
		AppKit.NSButton LoadTweets { get; set; }

		[Outlet]
		AppKit.NSTextField Search { get; set; }

		[Outlet]
		AppKit.NSTextField TitleLabel { get; set; }

		[Outlet]
		AppKit.NSTableView TweetTable { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (LoadTweets != null) {
				LoadTweets.Dispose ();
				LoadTweets = null;
			}

			if (Search != null) {
				Search.Dispose ();
				Search = null;
			}

			if (TweetTable != null) {
				TweetTable.Dispose ();
				TweetTable = null;
			}

			if (TitleLabel != null) {
				TitleLabel.Dispose ();
				TitleLabel = null;
			}
		}
	}
}
