using System;
using Acr.UserDialogs;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.InputMethods;
using Android.Widget;

using ImageSearch.Droid.Adapters;
using ImageSearch.ViewModel;

namespace ImageSearch.Droid
{
    [Activity(Label = "Image Search", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : BaseActivity
    {
        ProgressBar progressBar;
        ImageSearchViewModel viewModel;
        EditText editText;

        protected override int LayoutResource => Resource.Layout.main;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            viewModel = new ImageSearchViewModel();

            var adapter = new ImageAdapter(this, viewModel);

            var recyclerView = FindViewById<RecyclerView>(Resource.Id.ImageRecyclerView);

            recyclerView.SetAdapter(adapter);

            var layoutManager = new GridLayoutManager(this, 2);

            recyclerView.SetLayoutManager(layoutManager);

            progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            progressBar.Visibility = ViewStates.Gone;

            editText = FindViewById<EditText>(Resource.Id.SearchEditText);

            var clickButton = FindViewById<Button>(Resource.Id.GoButton);

            clickButton.Click += HandleButtonClicked;

            UserDialogs.Init(this);

            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(false);
        }

        async void HandleButtonClicked(object sender, EventArgs e)
        {
            //Dismiss Keyboard
            if (GetSystemService(InputMethodService) is InputMethodManager inputMethodManager && CurrentFocus?.WindowToken != null)
            {
                inputMethodManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
            }

            if (sender is Button clickButton)
            {
                clickButton.Enabled = false;
                progressBar.Visibility = ViewStates.Visible;

                await viewModel.SearchForImagesAsync(editText.Text.Trim());

                progressBar.Visibility = ViewStates.Gone;
                clickButton.Enabled = true;
            }
        }
    }
}

