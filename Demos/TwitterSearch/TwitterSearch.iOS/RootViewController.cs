using System;
using System.Drawing;

using Foundation;
using UIKit;
using TwitterSearch.Portable.ViewModels;
using SDWebImage;



namespace TwitterSearch.iOS
{
  public partial class RootViewController : UIViewController, IUITableViewDataSource
  {
    public RootViewController(IntPtr handle)
      : base(handle)
    {
    }

    

    public override void ViewDidLoad()
    {
      base.ViewDidLoad();
      activityIndicator = new UIActivityIndicatorView(new CoreGraphics.CGRect(0, 0, 20, 20));
      activityIndicator.ActivityIndicatorViewStyle = UIActivityIndicatorViewStyle.White;
      activityIndicator.HidesWhenStopped = true;
      NavigationItem.LeftBarButtonItem = new UIBarButtonItem(activityIndicator);
      TableViewTweets.WeakDataSource = this;
    }

    private UIActivityIndicatorView activityIndicator;
    private TwitterViewModel viewModel = new TwitterViewModel();


    async partial void ButtonGetTweets_TouchUpInside(UIButton sender)
    {
      ButtonGetTweets.Enabled = false;
      activityIndicator.StartAnimating();
      TextFieldSearch.ResignFirstResponder();


      await viewModel.LoadTweetsCommand(TextFieldSearch.Text.Trim());

      TableViewTweets.ReloadData();

      activityIndicator.StopAnimating();
      ButtonGetTweets.Enabled = true;
    }

    public UITableViewCell GetCell(UITableView tableView, NSIndexPath indexPath)
    {
      var cell = tableView.DequeueReusableCell("tweet");

      var tweet = viewModel.Tweets[indexPath.Row];

      cell.TextLabel.Text = tweet.DisplayName;
      cell.DetailTextLabel.Text = tweet.Text;

	  cell.ImageView.SetImage(
		  url: new NSUrl(tweet.Image),
		  placeholder: UIImage.FromBundle("Icon-Small.png")
	  );

      return cell;

    }

    public nint RowsInSection(UITableView tableView, nint section)
    {
      return viewModel.Tweets.Count;
    }


	public void RowSelected(UITableView tableView, NSIndexPath indexPath)
	{

		viewModel.Speak(indexPath.Row);

	}
  }
}