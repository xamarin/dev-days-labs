using System;
using Android.App;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Support.V7.Widget;
using ImageSearch.Droid.Adapters;
using ImageSearch.ViewModel;
using Acr.UserDialogs;

namespace ImageSearch.Droid
{
    [Activity(Label = "Image Search", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : BaseActivity
    {
        RecyclerView recyclerView;
        RecyclerView.LayoutManager layoutManager;
        ImageAdapter adapter;
        ProgressBar progressBar;

        ImageSearchViewModel viewModel;

        protected override int LayoutResource
        {
            get { return Resource.Layout.main; }
        }
        int count = 1;

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);


            viewModel = new ImageSearchViewModel();

            //Setup RecyclerView

            adapter = new ImageAdapter(this, viewModel);

            recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);

            recyclerView.SetAdapter(adapter);

            layoutManager = new GridLayoutManager(this, 2);

            recyclerView.SetLayoutManager(layoutManager);

            progressBar = FindViewById<ProgressBar>(Resource.Id.my_progress);
            progressBar.Visibility = ViewStates.Gone;

            var query = FindViewById<EditText>(Resource.Id.my_query);

            // Get our button from the layout resource,
            // and attach an event to it
            var clickButton = FindViewById<Button>(Resource.Id.my_button);

            //Button Click event to get images

            clickButton.Click += async (sender, args) =>
            {
                clickButton.Enabled = false;
                progressBar.Visibility = ViewStates.Visible;

                await viewModel.SearchForImagesAsync(query.Text.Trim());


                progressBar.Visibility = ViewStates.Gone;
                clickButton.Enabled = true;
            };

            UserDialogs.Init(this);
            SupportActionBar.SetDisplayHomeAsUpEnabled(false);
            SupportActionBar.SetHomeButtonEnabled(false);

        }
    }
}

