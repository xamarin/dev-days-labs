// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System;
using System.CodeDom.Compiler;

namespace Tasky
{
	[Register ("ViewController")]
	partial class ViewController
	{
		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITableView tableView { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UITextField textField { get; set; }

		[Outlet]
		[GeneratedCode ("iOS Designer", "1.0")]
		UIView textField { get; set; }

		void ReleaseDesignerOutlets ()
		{
			if (tableView != null) {
				tableView.Dispose ();
				tableView = null;
			}
			if (textField != null) {
				textField.Dispose ();
				textField = null;
			}
			if (textField != null) {
				textField.Dispose ();
				textField = null;
			}
		}
	}
}
