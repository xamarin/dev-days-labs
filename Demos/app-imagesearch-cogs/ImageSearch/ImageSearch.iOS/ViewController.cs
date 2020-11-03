using System;

using Foundation;

using ImageSearch.ViewModel;
using SDWebImage;

using UIKit;

namespace ImageSearch.iOS
{
    public partial class ViewController : UIViewController, IUICollectionViewDataSource
    {
        readonly ImageSearchViewModel _viewModel = new ImageSearchViewModel();

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CollectionViewImages.WeakDataSource = this;

            ButtonSearch.TouchUpInside += HandleButtonSearchTouchUpInside;
        }

        public nint GetItemsCount(UICollectionView collectionView, nint section) => _viewModel.Images.Count;

        public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (ImageCell)collectionView.DequeueReusableCell("imagecell", indexPath);

            var item = _viewModel.Images[indexPath.Row];

            if (item != null)
            {
                cell.Caption.Text = item.Name;
                cell.Image.SetImage(new NSUrl(item.ContentUrl));
            }

            return cell;
        }

        async void HandleButtonSearchTouchUpInside(object sender, EventArgs e)
        {
            //Dismiss Keyboard
            View?.EndEditing(true);

            ButtonSearch.Enabled = false;
            ActivityIsLoading.StartAnimating();

            var text = TextFieldQuery.Text;

            if (text != null && text != " ")
            {
                await _viewModel.SearchForImagesAsync(text);
                CollectionViewImages.ReloadData();
            }

            ButtonSearch.Enabled = true;
            ActivityIsLoading.StopAnimating();
        }
    }
}

