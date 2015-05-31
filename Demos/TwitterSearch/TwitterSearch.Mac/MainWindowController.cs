using System;

using Foundation;
using AppKit;
using TwitterSearch.Portable.ViewModels;
using CoreGraphics;

namespace TwitterSearch.Mac
{
	public partial class MainWindowController : NSWindowController, INSTableViewDataSource, INSTableViewDelegate
	{
		public MainWindowController (IntPtr handle) : base (handle)
		{
		}

		[Export ("initWithCoder:")]
		public MainWindowController (NSCoder coder) : base (coder)
		{
		}

		public MainWindowController () : base ("MainWindow")
		{
		}

		TwitterViewModel viewModel;
		bool IsBusy;

		public override void AwakeFromNib ()
		{
			base.AwakeFromNib ();

			viewModel = new TwitterViewModel();

			Search.EditingEnded += OnLoadPressed;
			LoadTweets.Activated += OnLoadPressed;
			TweetTable.WeakDataSource = this;
			TweetTable.WeakDelegate = this;
			TweetTable.HeaderView = null;
		}

		async void OnLoadPressed (object o, EventArgs e)
		{
			if (IsBusy)
				return;
			
			IsBusy = true;
			LoadTweets.Enabled = false;
			Search.ResignFirstResponder ();
			await viewModel.LoadTweetsCommand (Search.StringValue.Trim ());
			TweetTable.ReloadData ();
			TweetTable.SizeToFit ();
			LoadTweets.Enabled = true;
			IsBusy = false;
		}

		[Export ("numberOfRowsInTableView:")]
		public System.nint GetRowCount (AppKit.NSTableView tableView)
		{
			return viewModel.Tweets.Count;
		}

		[Export ("tableView:heightOfRow:")]
		public System.nfloat GetRowHeight (AppKit.NSTableView tableView, System.nint row)
		{
			return 50;
		}

		const string identifer = "myCellIdentifier";

		[Export ("tableView:viewForTableColumn:row:")]
		public AppKit.NSView GetViewForItem (AppKit.NSTableView tableView, AppKit.NSTableColumn tableColumn, System.nint row)
		{
			if (viewModel.Tweets.Count < row)
				return null;

			TweetView view = (TweetView)tableView.MakeView (identifer, this);
			if (view == null) {
				TweetViewController c = new TweetViewController ();
				view = c.View;
				view.Frame = new CGRect (0, 0, tableView.Frame.Width, 0);
				view.Identifier = identifer;
			}
			view.Tweet = viewModel.Tweets [(int)row];

			return view;
		}

		public new MainWindow Window {
			get { return (MainWindow)base.Window; }
		}
	}
}
