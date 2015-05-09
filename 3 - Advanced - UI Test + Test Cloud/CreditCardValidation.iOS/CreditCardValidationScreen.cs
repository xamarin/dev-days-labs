using System;
using CoreGraphics;

using UIKit;

namespace CreditCardValidation.iOS
{
    public class CreditCardValidationScreen : UIViewController
    {
		UITextView creditCardTextField;
		UILabel errorMessagesTextField;
		UIButton validateButton;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            Title = "Credit Card Validation";
            View.BackgroundColor = UIColor.White;


            var frame = new CGRect(10, 130, 300, 30);

            creditCardTextField = new UITextView(frame);
			creditCardTextField.AccessibilityIdentifier  = ("CreditCardTextField");
            creditCardTextField.Layer.BorderColor = UIColor.Black.CGColor;
            creditCardTextField.Layer.BorderWidth = 0.5f;
            creditCardTextField.Layer.CornerRadius = 5f;
            creditCardTextField.Font = UIFont.SystemFontOfSize(16);
            creditCardTextField.TextContainer.MaximumNumberOfLines = 1;
            creditCardTextField.KeyboardType = UIKeyboardType.NumberPad;

            validateButton = new UIButton(new CGRect(10, 165, 300, 40));
            validateButton.SetTitle("Validate Credit Card", UIControlState.Normal);
			validateButton.AccessibilityIdentifier  = ("ValidateButton");
            validateButton.SetTitleColor(UIColor.White, UIControlState.Normal);
            validateButton.BackgroundColor = UIColor.FromRGB(52, 152, 219);
            validateButton.Layer.CornerRadius = 5;

            errorMessagesTextField = new UILabel(new CGRect(10, 210, 300, 40));
			errorMessagesTextField.AccessibilityIdentifier  = ("ErrorMessagesTextField");
            errorMessagesTextField.Text = String.Empty;

            validateButton.TouchUpInside += (object sender, EventArgs e) =>{
                                                 errorMessagesTextField.Text = String.Empty;

                                                 // perform a simple "required" validation
                                                 string errMessage;
                                                 if (!IsCCValid(out errMessage))
                                                 {
                                                     // need to update on the main thread to change the border color
                                                     InvokeOnMainThread(() =>{
                                                                            creditCardTextField.BackgroundColor = UIColor.Yellow;
                                                                            creditCardTextField.Layer.BorderColor = UIColor.Red.CGColor;
                                                                            creditCardTextField.Layer.BorderWidth = 3;
                                                                            creditCardTextField.Layer.CornerRadius = 5;
                                                                            errorMessagesTextField.Text = errMessage;
                                                                        });
                                                 }
                                                 else
                                                 {
                                                     NavigationController.PushViewController(new CreditCardValidationSuccess(), true);
                                                 }
                                             };

            View.Add(creditCardTextField);
            View.Add(validateButton);
            View.Add(errorMessagesTextField);
        }

        bool IsCCValid(out string errMessage)
        {
            errMessage = "";

            if ((creditCardTextField.Text.Length < 16) || (string.IsNullOrWhiteSpace(creditCardTextField.Text)))
            {
                errMessage = "Credit card number is to short.";
                return false;
            }
            if (creditCardTextField.Text.Length > 16)
            {
                errMessage = "Credit card number is to long.";
                return false;
            }

            return true;
        }
    }
}
