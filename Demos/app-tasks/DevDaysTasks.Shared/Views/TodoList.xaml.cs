using System;

using Xamarin.Forms;

namespace DevDaysTasks
{
    public partial class TodoList : ContentPage
    {
        TodoListViewModel viewModel;

        public TodoList()
        {
            InitializeComponent();
            BindingContext = viewModel = new TodoListViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            // Refresh the list of items whenever the page appears without syncing data with the server
            viewModel?.RefreshCommand?.Execute(false);
        }

        // Event handlers
        async void OnSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var list = (ListView)sender;
            var todo = e.SelectedItem as TodoItem;

            if (!Device.RuntimePlatform.Equals(Device.iOS) && todo != null)
            {
                // Not iOS - the swipe-to-delete is discoverable there
				if (Device.RuntimePlatform.Equals(Device.Android))
                {
                    await DisplayAlert(todo.Name, "Press-and-hold to complete task " + todo.Name, "Got it!");
                }
                else
                {
                    // Windows only, not all platforms support the Context Actions yet
					var isAccepted = await DisplayAlert("Mark completed?", "Do you wish to complete " + todo.Name + "?", "Complete", "Cancel");
					if (isAccepted)
                        viewModel.CompleteCommand.Execute(todo);
                }
            }

            // prevents background getting highlighted
            list.SelectedItem = null;
        }

        // http://developer.xamarin.com/guides/cross-platform/xamarin-forms/working-with/listview/#context
        void OnComplete(object sender, EventArgs e)
        {
            // Get selected item
            var menuItem = ((MenuItem)sender);
            var todoItem = menuItem.CommandParameter as TodoItem;

            // Mark as completed by executing the according command
            viewModel?.CompleteCommand?.Execute(todoItem);
        }
    }
}

