# Hello.iOS Quickstart

In this walkthrough we’ll create an application that translates an alphanumeric phone number entered by the user into a numeric phone number, and then calls that number. The final application looks like this:

 [ ![](Images/image1.png)](Images/image1.png)

Let’s get started!


# Requirements

iOS development with Xamarin requires:

-  A Mac running OS X Mountain Lion or above.
-  Latest version of XCode and iOS SDK installed from the  [App Store](https://itunes.apple.com/us/app/xcode/id497799835?mt=12) .


Xamarin.iOS works with any of the following setups:

-  Latest version of Xamarin Studio on a Mac that fits the above specifications.
-  Latest version of Visual Studio Professional or higher on Windows 7 or above, paired with a Mac build host that fits the above specifications.  *This setup requires a Xamarin Business License or  [trial](http://developer.xamarin.com/guides/cross-platform/getting_started/beginning_a_xamarin_trial/).*


The [Xamarin.iOS OSX Installation guide](http://developer.xamarin.com/guides/ios/getting_started/installation/mac/) is available for step-by-step installation instructions

<div class="note"><p>Note: Xamarin.iOS is not available for Xamarin Studio on Windows.</p></div>

Before we get started, please download the [Xamarin App Icons and Launch Screens set](Resources/Xamarin App Icons and Launch Images.zip).

<ide name="xs">
<h1>Xamarin Studio Walkthrough</h1>

<p>In this walkthrough we’ll create an application called Phoneword that translates an alphanumeric phone number into a numeric phone number.</p>

<ol>
<li><p>Let’s launch Xamarin Studio from the <span class="uiitem">Applications</span> folder or <span class="uiitem">Spotlight</span> to bring up the Launch screen:</p>

<p><a href="Images/image2New.png" class="fancybox"><img src="Images/image2New.png"></a></p>
 
<p>In the top left, let’s click <span class="uiitem">New Solution...</span> to create a new Xamarin.iOS solution:</p>
<p><a href="Images/image3New.png" class="fancybox"><img src="Images/image3New.png" width="311" height="195"></a></p>
</li>
 
<li><p>From the <span class="uiitem">New Solution dialog</span>, we’ll choose the <span class="uiitem">iOS > App > Single View Application</span> template, ensuring that C# is selected. Click <span class="uiitem">Next</span>:
<p><a href="Images/image4New.png" class=" fancybox"><img src="Images/image4New.png"></a><br />.</li>

<li>Let's configure the app. Give it the <span class="uiitem">Name</span> <code>Phoneword_iOS</code>, and leave <span class="uiitem">Identifiers</span>, <span class="uiitem">Devices</span>, and <span class="uiitem">Target</span> as default. Click <span class="uiitem">Next</span>:
<p><a href="Images/image5New.png" class=" fancybox"><img src="Images/image5New.png"></a><br /></li>

<li>Let's leave the Project and Solution Name as is. We can choose the location of our project here, or keep it as the default:
<p><a href="Images/image6New.png" class=" fancybox"><img src="Images/image6New.png"></a><br /></li> 

<li>Click Create to make the <span class="uiitem">Solution</span>. <p>Note that creating a Solution automatically creates a <a href="http://developer.xamarin.com/guides/testcloud/uitest/intro-to-uitest/">UITest</a> project. This is beyond the scope of this document, for more information on using UITest, you can read relevant documentation <a href="/guides/testcloud/uitest/intro-to-uitest/">here</a>.</p>
 
<li><p>Next, we’ll open the <span class="uiitem">Main.storyboard</span> file by double-clicking on it in the <span class="uiitem">Solution Pad</span>. This will allow us to create our UI:</p>
<p><a href="Images/image7New.png" class=" fancybox"><img src="Images/image7New.png"></a></p>
<p>Note that size classes are enabled by default. You can read more about them <a href="http://developer.xamarin.com/guides/ios/platform_features/introduction_to_Unified_Storyboards/#Unified_Storyboards">here.</a></p>
</li>
 
<li><p>From the <span class="uiitem">Toolbox</span> (the area on the right), let’s type "label" into the search bar and drag a <span class="uiitem">Label</span> onto the design surface (the area in the center):</p>
<p><a href="Images/image8New.png" class=" fancybox"><img src="Images/image8New.png"></a></p>
</li>
 
<li><p>Next, grab the handles of the <em>Dragging Controls</em> (the circles around the control) and make the label wider:</p>
<p><a href="Images/image9.png" class=" fancybox"><img src="Images/image9.png"></a></p>
</li>
 
<li><p>With the <span class="uiitem">Label</span> selected on the design surface, use the <span class="uiitem">Properties Pad</span> to change the <span class="uiitem">Title</span> property of the <span class="uiitem">Label</span> to "Enter a Phoneword:"</p>
<p><a href="Images/image10.png" class=" fancybox"><img src="Images/image10.png"></a></p>
 
<div class="note"><strong>Note</strong>: You can bring up the <span class="uiitem">Properties Pad</span> or <span class="uiitem">Toolbox</span> at any time by navigating to <span class="uiitem">View > Pads</span>.
</div>
</li>
 
<li><p>Next, let’s search for “text field” inside the Toolbox and drag a <span class="uiitem">Text Field</span> from the <span class="uiitem">Toolbox</span> onto the design surface and place it under the <span class="uiitem">Label</span>. Adjust the width until the <span class="uiitem">Text Field</span> is the same width as the <span class="uiitem">Label</span>:</p>
<p><a href="Images/image12New.png" class=" fancybox"><img src="Images/image12New.png"></a></p>
</li>
 
<li><p>With the <span class="uiitem">Text Field</span> selected on the design surface, change the <span class="uiitem">Text Field</span>’s <span class="uiitem">Name</span> property in the Identity section of the <span class="uiitem">Properties Pad</span> to <code>PhoneNumberText</code>, and change the <span class="uiitem">Title</span> property to "1-855-XAMARIN":</p>
<p><a href="Images/image13New.png" class=" fancybox"><img src="Images/image13New.png"></a></p>
</li>
 
<li><p>Next, drag a <span class="uiitem">Button</span> from the <span class="uiitem">Toolbox</span> onto the design surface and place it under the <span class="uiitem">Text Field</span>. Adjust the width so the <span class="uiitem">Button</span> is as wide as the <span class="uiitem">Text Field</span> and <span class="uiitem">Label</span>:</p>
<p><a href="Images/image14New.png" class=" fancybox"><img src="Images/image14New.png"></a></p>
</li>
 
<li><p>With the <span class="uiitem">Button</span> selected on the design surface, change the <span class="uiitem">Name</span> property in the <span class="uiitem">Identity</span> section of the <span class="uiitem">Properties Pad</span> to <code>TranslateButton</code>. Change the <span class="uiitem">Title</span> property to "Translate":</p>
<p><a href="Images/image15New.png" class=" fancybox"><img src="Images/image15New.png"></a></p>
</li>
 
<li><p>Let’s repeat the two steps above and drag a <span class="uiitem">Button</span> from the <span class="uiitem">Toolbox</span> onto the design surface and place it under the first <span class="uiitem">Button</span>. Adjust the width so the <span class="uiitem">Button</span> is as wide as the first <span class="uiitem">Button</span>:</p>
<p><a href="Images/image16New.png" class=" fancybox"><img src="Images/image16New.png"></a></p>
</li>
 
<li><p>With the second <span class="uiitem">Button</span> selected on the design surface, we’ll change the <span class="uiitem">Name</span> property in the <span class="uiitem">Identity</span> section of the <span class="uiitem">Properties Pad</span> to <code>CallButton</code>. We’ll change the <span class="uiitem">Title</span> property to "Call":</p>
<p><a href="Images/image17New.png" class=" fancybox"><img src="Images/image17New.png"></a></p>
 
<p>Let’s save our work by navigating to <span class="uiitem">File > Save</span> or by pressing <span class="uiitem">⌘ + s</span>.</p>
</li>

<li><p>Now, let’s add some code to translate phone numbers from alphanumeric to numeric. We’ll add a new file to the Project by clicking on the gray gear icon next to the <span class="uiitem">Phoneword_iOS</span> Project in the <span class="uiitem">Solution Pad</span> and choosing <span class="uiitem">Add > New File...</span> or pressing <span class="uiitem">⌘ + n</span>:</p>
<p><a href="Images/image18.png" class=" fancybox"><img src="Images/image18.png"></a></p>
</li>
 
<li><p>In the <span class="uiitem">New File</span> dialog, select <span class="uiitem">General > Empty Class</span> and name the new file <code>PhoneTranslator</code>:</p>
<p><a href="Images/image19.png" class=" fancybox"><img src="Images/image19.png"></a></p>
</li>
 
<li><p>This creates a new, empty C# class for us. Remove all the template code and replace it with the following code:</p>

<pre><code class=" syntax brush-C#">
using System.Text;
using System;

namespace Phoneword_iOS
{
	public static class PhoneTranslator
	{
		public static string ToNumber(string raw)
		{
			if (string.IsNullOrWhiteSpace(raw))
				return "";
			else
				raw = raw.ToUpperInvariant();

			var newNumber = new StringBuilder();
			foreach (var c in raw)
			{
				if (" -0123456789".Contains(c))
					newNumber.Append(c);
				else {
					var result = TranslateToNumber(c);
					if (result != null)
						newNumber.Append(result);
				}    
				// otherwise we've skipped a non-numeric char
			}
			return newNumber.ToString();
		}

		static bool Contains (this string keyString, char c)
		{
			return keyString.IndexOf(c) >= 0;
		}

		static int? TranslateToNumber(char c)
		{
			if ("ABC".Contains(c))
				return 2;
			else if ("DEF".Contains(c))
				return 3;
			else if ("GHI".Contains(c))
				return 4;
			else if ("JKL".Contains(c))
				return 5;
			else if ("MNO".Contains(c))
				return 6;
			else if ("PQRS".Contains(c))
				return 7;
			else if ("TUV".Contains(c))
				return 8;
			else if ("WXYZ".Contains(c))
				return 9;
			return null;
		}
	}
}
</code></pre>

<p>Save the <span class="uiitem">PhoneTranslator.cs</span> file and close it.</p></li>

<li><p>Next we’re going to add code to wire up the user interface. Let’s add the code to power the user interface into the <code>ViewController</code> class. Double-click on <span class="uiitem">ViewController.cs</span> in the <span class="uiitem">Solution Pad</span> to open it:</p>
<p><a href="Images/image20New.png" class=" fancybox"><img src="Images/image20New.png"></a></p>
</li>
 
<li><p>Let’s begin by wiring up the <code>TranslateButton</code>. In the <span class="uiitem">ViewController</span> class, find the <code>ViewDidLoad</code> method. We will add our button code inside <code>ViewDidLoad</code>, below the <code>base.ViewDidLoad()</code> call:</p>

<pre><code class=" syntax brush-C#">public override void ViewDidLoad ()
{
  base.ViewDidLoad ();
  // code goes here
}</code></pre>
</li>

<li><p>Let’s add code to respond to the user pressing the first button, which we’ve named <code>TranslateButton</code>. Populate <code>ViewDidLoad</code> with the following code:</p>

<pre><code class=" syntax brush-C#">string translatedNumber = "";

TranslateButton.TouchUpInside += (object sender, EventArgs e) => {
				// Convert the phone number with text to a number 
				// using PhoneTranslator.cs
				translatedNumber = PhoneTranslator.ToNumber(
					PhoneNumberText.Text); 
				
				// Dismiss the keyboard if text field was tapped
				PhoneNumberText.ResignFirstResponder ();

				if (translatedNumber == "") {
					CallButton.SetTitle ("Call ", UIControlState.Normal);
					CallButton.Enabled = false;
				} else {
					CallButton.SetTitle ("Call " + translatedNumber, 
						UIControlState.Normal);
					CallButton.Enabled = true;
				}
			};
</code></pre>
</li>

<li><p>Next let’s add code to respond to the user pressing the second button, which we’ve named <code>CallButton</code>. Place the following code below the code for the <code>TranslateButton</code>:</p>

<pre><code class=" syntax brush-C#">CallButton.TouchUpInside += (object sender, EventArgs e) => {
				// Use URL handler with tel: prefix to invoke Apple's Phone app...
				var url = new NSUrl ("tel:" + translatedNumber);
				
				
				// ...otherwise show an alert dialog                
				if (!UIApplication.SharedApplication.OpenUrl (url)) {
					var alert = UIAlertController.Create ("Not supported", "Scheme 'tel:' is not supported on this device", UIAlertControllerStyle.Alert);
					alert.AddAction (UIAlertAction.Create ("Ok", UIAlertActionStyle.Default, null));
					PresentViewController (alert, true, null);
				}
			};
</code></pre>
</li>

<li><p>Let’s save our work, and then build the application by choosing <span class="uiitem">Build > Build All</span> or pressing <span class="uiitem">⌘ + B</span>.  If our application compiles, we will get a success message at the top of the IDE:</p>

<p><a href="Images/image21.png" class=" fancybox"><img src="Images/image21.png"></a></p>
 
<p>If there are errors, we can go through the previous steps and correct any mistakes until the application builds successfully.</p></li>

<li><p>We now have a working application and it’s time to add the finishing touches! We can edit the application name and icons in the <span class="uiitem">Info.plist</span> file. Let’s open <span class="uiitem">Info.plist</span> by double-clicking on it in the <span class="uiitem">Solution Pad</span>:</p>
<p><a href="Images/image22New.png" class=" fancybox"><img src="Images/image22New.png"></a></p>
</li>
 
<li><p>In the <span class="uiitem">iOS Application Target</span> section, let’s change the<span class="uiitem"> Application Name</span> to "Phoneword":</p>
<p><a href="Images/image23.png" class=" fancybox"><img src="Images/image23.png"></a></p>
</li>
 
<li><p>Next, let’s set the launch images. We’ll assign <span class="uiitem">Default.png</span> to the standard resolution (320x480) placeholder, <span class="uiitem">Default@2x.png</span> to the <span class="uiitem">Retina (3.5-inch)</span> placeholder, and finally <span class="uiitem">Default-568h@2x.png</span> to the <span class="uiitem">Retina (4-inch)</span> placeholder:</p>
<p><a href="Images/image26.png" class=" fancybox"><img src="Images/image26.png"></a></p>

<li><p>To set application icons and launch images, first download the <a href="Resources/Xamarin App Icons and Launch Images.zip">Xamarin App Icons & Launch Screens set</a>. As we are using <a href="http://developer.xamarin.com/guides/ios/application_fundamentals/working_with_images/app-icons/#Managing_Icons_with_Asset_Catalogs">Asset Catalog</a> to manage our icons, navigate to <span class="uiitem">Resources > Images.xcassets > AppIcons.appiconset</span> in our <span class="uiitem">Solution Pad</span>, and the open <code>contents.json</code> file. In the iPhone Icons section, click directly on the (57x57) icon placeholder and select the matching icon from the Xamarin App Icons & Launch Images directory:</p>
<p><a href="Images/image24New.png" class=" fancybox"><img src="Images/image24New.png"></a></p>
 
<p>Let’s continue filling in all icons. Xamarin Studio will replace the placeholders with our icons:</p>
<p><a href="Images/image25New.png" class=" fancybox"><img src="Images/image25New.png"></a></p>
</li>
 
<p>The image names follow iOS naming conventions for images of different densities.</p></li>

<li><p>Finally, let’s test our application in the <span class="uiitem">iOS Simulator</span>. In the top left of the IDE, choose <span class="uiitem">Debug</span> from the first dropdown, and <span class="uiitem">iPhone 6 iOS 8.x</span> from the second dropdown, and press <span class="uiitem">Start</span> (the triangular button that resembles a Play button):</p>
<p><a href="Images/image27New.png" class=" fancybox"><img src="Images/image27New.png"></a></p>
</li>
 
<li><p>This will launch our application inside the iOS Simulator:</p>
<p><a href="Images/image28.png" class=" fancybox"><img src="Images/image28.png"></a></p>
 
<p>Phone calls are not supported in the iOS Simulator; instead, we’ll see our alert dialog when we try to place a call:</p>
<p><a href="Images/image29.png" class=" fancybox"><img src="Images/image29.png"></a></p>
</li>
</ol>
 

</ide>


Congratulations on completing your first Xamarin.iOS application! Now it’s time to dissect the tools and skills we just learned in the [Hello, iOS Deep Dive](http://developer.xamarin.com/guides/ios/getting_started/hello,_iOS/hello,_iOS_deepdive).
