using System;

using Xamarin.Forms;

namespace DevDaysTasks
{
    public partial class TodoListPage : ContentPage
    {
        readonly TodoListViewModel viewModel;

        public TodoListPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new TodoListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Refresh the list of items whenever the page appears without syncing data with the server
            viewModel.RefreshCommand?.Execute(false);
        }

        // Event handlers
        async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (sender is ListView listView && e.SelectedItem is TodoItem todoItem)
            {
                if (Device.RuntimePlatform is Device.Android)
                {
                    await DisplayAlert(todoItem.Name, "Press-and-hold to complete task " + todoItem.Name, "Got it!");
                }
                else if (Device.RuntimePlatform is Device.UWP)
                {
                    // Windows only, not all platforms support the Context Actions yet
                    var isAccepted = await DisplayAlert("Mark completed?", "Do you wish to complete " + todoItem.Name + "?", "Complete", "Cancel");
                    if (isAccepted)
                        viewModel.CompleteCommand.Execute(todoItem);
                }

                // prevents background getting highlighted
                listView.SelectedItem = null;
            }

        }
    }
}
