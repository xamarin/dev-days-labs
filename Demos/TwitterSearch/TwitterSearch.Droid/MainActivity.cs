using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;
using TwitterSearch.Portable.ViewModels;

namespace TwitterSearch.Droid
{
  [Activity(Label = "Twitter Search", MainLauncher = true, Icon = "@drawable/icon")]
  public class MainActivity : AppCompatActivity
  {

    TwitterViewModel viewModel = new TwitterViewModel();
    protected override void OnCreate(Bundle bundle)
    {
      base.OnCreate(bundle);

      // Set our view from the "main" layout resource
      SetContentView(Resource.Layout.Main);

      var search = FindViewById<EditText>(Resource.Id.search_text);
      var button = FindViewById<Button>(Resource.Id.get_tweets);
      var listView = FindViewById<ListView>(Resource.Id.listView);
	  var progress = FindViewById<ProgressBar>(Resource.Id.progressBar);
	  progress.Indeterminate = true;
	  progress.Visibility = ViewStates.Invisible;


      button.Click += async (sender, args) =>
        {

          button.Enabled = false;

		  progress.Visibility = ViewStates.Visible;

          await viewModel.LoadTweetsCommand(search.Text.Trim());


          listView.Adapter = new TweetAdapter(this, viewModel);

		  progress.Visibility = ViewStates.Invisible;
          button.Enabled = true;


        };

      listView.ItemClick += (sender, args) =>
        {
          viewModel.Speak(args.Position);
        };


    }
  }
}

