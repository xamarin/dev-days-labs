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
    [Register ("ViewController")]
    partial class ViewController
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIActivityIndicatorView ActivityIsLoading { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton ButtonSearch { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UICollectionView CollectionViewImages { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UITextField TextFieldQuery { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (ActivityIsLoading != null) {
                ActivityIsLoading.Dispose ();
                ActivityIsLoading = null;
            }

            if (ButtonSearch != null) {
                ButtonSearch.Dispose ();
                ButtonSearch = null;
            }

            if (CollectionViewImages != null) {
                CollectionViewImages.Dispose ();
                CollectionViewImages = null;
            }

            if (TextFieldQuery != null) {
                TextFieldQuery.Dispose ();
                TextFieldQuery = null;
            }
        }
    }
}