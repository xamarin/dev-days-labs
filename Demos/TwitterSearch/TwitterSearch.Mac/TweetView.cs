using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;
using TwitterSearch.Portable.Models;
using System.Threading.Tasks;

namespace TwitterSearch.Mac
{
	public partial class TweetView : AppKit.NSView
	{
		#region Constructors

		// Called when created from unmanaged code
		public TweetView (IntPtr handle) : base (handle)
		{
			Initialize ();
		}

		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public TweetView (NSCoder coder) : base (coder)
		{
			Initialize ();
		}

		// Shared initialization code
		void Initialize ()
		{
		}

		#endregion

		public Tweet Tweet { get; set; }

		static Dictionary<string, NSImage> cachedImages = new Dictionary<string, NSImage> ();
		async Task LoadImage ()
		{
			NSData d = await Task.Factory.StartNew<NSData> (() => NSData.FromUrl (new NSUrl (Tweet.Image)));
			NSImage i = new NSImage (d);
			if (!cachedImages.ContainsKey (Tweet.Image))
				cachedImages.Add (Tweet.Image, i);
			Image.Image = i;
		}

		public override async void ViewDidMoveToSuperview ()
		{
			Title.StringValue = string.Format ("{0} at {1}", Tweet.ScreenName, Tweet.Date);
			Text.StringValue = Tweet.Text;

			// We don't have SDWebImage to do caching for us
			if (cachedImages.ContainsKey (Tweet.Image))
				Image.Image = cachedImages [Tweet.Image];
			else
				await LoadImage ();
		}
	}
}
