using Foundation;
using System;
using UIKit;

namespace ImageSearch.iOS
{
    public partial class ImageCell : UICollectionViewCell
    {
        public ImageCell (IntPtr handle) : base (handle)
        {
        }

        public UIImageView Image => MainImage;

        public UILabel Caption => LabelCaption;
    }
}