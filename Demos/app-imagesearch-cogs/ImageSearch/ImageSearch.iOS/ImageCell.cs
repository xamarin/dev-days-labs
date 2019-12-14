using System;

using UIKit;
using Foundation;

namespace ImageSearch.iOS
{
    public partial class ImageCell : UICollectionViewCell
    {
        public ImageCell(IntPtr handle) : base(handle)
        {
        }

        public UIImageView Image => MainImage;

        public LabelWithBorder Caption => LabelCaption;
    }

    [Register(nameof(LabelWithBorder))]
    public class LabelWithBorder : UILabel
    {
        readonly UIEdgeInsets EdgeInsets = new UIEdgeInsets(5, 5, 5, 5);
        readonly UIEdgeInsets InverseEdgeInsets = new UIEdgeInsets(-5, -5, -5, -5);

        public LabelWithBorder(IntPtr handle) : base(handle)
        {
        }

        public override CoreGraphics.CGRect TextRectForBounds(CoreGraphics.CGRect bounds, nint numberOfLines)
        {
            var textRect = base.TextRectForBounds(EdgeInsets.InsetRect(bounds), numberOfLines);
            return InverseEdgeInsets.InsetRect(textRect);
        }

        public override void DrawText(CoreGraphics.CGRect rect) => base.DrawText(EdgeInsets.InsetRect(rect));
    }
}