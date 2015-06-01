using System;
using System.Collections.Generic;
using System.Linq;
using Foundation;
using AppKit;

namespace TwitterSearch.Mac
{
	public partial class TweetViewController : AppKit.NSViewController
	{
		#region Constructors

		// Called when created from unmanaged code
		public TweetViewController (IntPtr handle) : base (handle)
		{
			Initialize ();
		}

		// Called when created directly from a XIB file
		[Export ("initWithCoder:")]
		public TweetViewController (NSCoder coder) : base (coder)
		{
			Initialize ();
		}

		// Call to load from the XIB/NIB file
		public TweetViewController () : base ("TweetView", NSBundle.MainBundle)
		{
			Initialize ();
		}

		// Shared initialization code
		void Initialize ()
		{
		}

		#endregion

		//strongly typed view accessor
		public new TweetView View {
			get {
				return (TweetView)base.View;
			}
		}
	}
}
