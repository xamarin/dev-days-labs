// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace ImageSearch.iOS
{
    [Register ("ImageCell")]
    partial class ImageCell
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UILabel LabelCaption { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView MainImage { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (LabelCaption != null) {
                LabelCaption.Dispose ();
                LabelCaption = null;
            }

            if (MainImage != null) {
                MainImage.Dispose ();
                MainImage = null;
            }
        }
    }
}