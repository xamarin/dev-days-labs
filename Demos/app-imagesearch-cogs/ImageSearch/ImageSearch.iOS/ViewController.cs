using System;

using Foundation;

using ImageSearch.ViewModel;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;
using SDWebImage;

using UIKit;

namespace ImageSearch.iOS
{
    public partial class ViewController : UIViewController, IUICollectionViewDataSource
    {
        readonly ImageSearchViewModel viewModel = new ImageSearchViewModel();

        public ViewController(IntPtr handle) : base(handle)
        {
        }

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            CollectionViewImages.WeakDataSource = this;

            ButtonSearch.TouchUpInside += HandleButtonSearchTouchUpInside;
        }

        public nint GetItemsCount(UICollectionView collectionView, nint section) => viewModel.Images.Count;

        public UICollectionViewCell GetCell(UICollectionView collectionView, NSIndexPath indexPath)
        {
            var cell = (ImageCell)collectionView.DequeueReusableCell("imagecell", indexPath);

            var item = viewModel.Images[indexPath.Row];

            cell.Caption.Text = item?.Name;

            cell.Image.SetImage(new NSUrl(item?.ContentUrl));

            return cell;
        }

        async void HandleButtonSearchTouchUpInside(object sender, EventArgs e)
        {
            //Dismiss Keyboard
            View.EndEditing(true);

            ButtonSearch.Enabled = false;
            ActivityIsLoading.StartAnimating();

            await viewModel.SearchForImagesAsync(TextFieldQuery.Text);
            CollectionViewImages.ReloadData();

            ButtonSearch.Enabled = true;
            ActivityIsLoading.StopAnimating();
        }
    }
}

