using Acr.UserDialogs;

using Android.App;
using Android.OS;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Widget;

using ImageSearch.Droid.Adapters;
using ImageSearch.ViewModel;

namespace ImageSearch.Droid
{
	[Activity(Label = "Image Search", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : BaseActivity
    {
        protected override int LayoutResource => Resource.Layout.main;

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

            var viewModel = new ImageSearchViewModel();

			var adapter = new ImageAdapter(this, viewModel);

			var recyclerView = FindViewById<RecyclerView>(Resource.Id.recyclerView);
			
			recyclerView.SetAdapter(adapter);
			
			var layoutManager = new GridLayoutManager(this, 2);
			
			recyclerView.SetLayoutManager(layoutManager);
			
			var progressBar = FindViewById<ProgressBar>(Resource.Id.my_progress);
			progressBar.Visibility = ViewStates.Gone;

			var query = FindViewById<EditText>(Resource.Id.my_query);

			var clickButton = FindViewById<Button>(Resource.Id.my_button);

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

