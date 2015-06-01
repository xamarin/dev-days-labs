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
	[Register ("TweetView")]
	partial class TweetView
	{
		[Outlet]
		AppKit.NSImageView Image { get; set; }

		[Outlet]
		AppKit.NSTextField Text { get; set; }

		[Outlet]
		AppKit.NSTextField Title { get; set; }
		
		void ReleaseDesignerOutlets ()
		{
			if (Title != null) {
				Title.Dispose ();
				Title = null;
			}

			if (Text != null) {
				Text.Dispose ();
				Text = null;
			}

			if (Image != null) {
				Image.Dispose ();
				Image = null;
			}
		}
	}
}
