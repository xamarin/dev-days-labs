using System;

using Acr.UserDialogs;

using Android.App;
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
        readonly ImageSearchViewModel _viewModel = new ImageSearchViewModel();

        ProgressBar? _progressBar;
        EditText? _editText;

        protected override int LayoutResource => Resource.Layout.main;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            var adapter = new ImageAdapter(this, _viewModel);

            var recyclerView = FindViewById<RecyclerView>(Resource.Id.ImageRecyclerView);

            recyclerView.SetAdapter(adapter);

            var layoutManager = new GridLayoutManager(this, 2);

            recyclerView.SetLayoutManager(layoutManager);

            _progressBar = FindViewById<ProgressBar>(Resource.Id.ProgressBar);
            _progressBar.Visibility = ViewStates.Gone;

            _editText = FindViewById<EditText>(Resource.Id.SearchEditText);

            var clickButton = FindViewById<Button>(Resource.Id.GoButton);
            clickButton.Click += HandleButtonClicked;

            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(false);
        }

        async void HandleButtonClicked(object sender, EventArgs e)
        {
            DismissKeyboard();

            var clickButton = (Button)sender;

            clickButton.Enabled = false;

            if (_progressBar != null)
                _progressBar.Visibility = ViewStates.Visible;

            await _viewModel.SearchForImagesAsync(_editText?.Text.Trim() ?? string.Empty);

            if (_progressBar != null)
                _progressBar.Visibility = ViewStates.Gone;

            clickButton.Enabled = true;
        }

        void DismissKeyboard()
        {
            if (GetSystemService(InputMethodService) is InputMethodManager inputMethodManager && CurrentFocus?.WindowToken != null)
            {
                inputMethodManager.HideSoftInputFromWindow(CurrentFocus.WindowToken, HideSoftInputFlags.NotAlways);
            }
        }
    }
}

