using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace DevDaysTasks
{
	public class TodoListViewModel : INotifyPropertyChanged
	{
		TodoItemManager manager;
		ObservableCollection<TodoItem> items;
		string name;
		bool isBusy;


		public TodoListViewModel()
		{
			manager = TodoItemManager.DefaultManager;
			Items = new ObservableCollection<TodoItem>();

			AddCommand = new Command(async () => await AddAsync());
			CompleteCommand = new Command<TodoItem>(async (item) => await CompleteAsync(item));
			SyncCommand = new Command(async () => await RefreshAsync(true));
			RefreshCommand = new Command(async () => await RefreshAsync(false));
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public Command<TodoItem> CompleteCommand { get; }
		public Command SyncCommand { get; }
		public Command RefreshCommand { get; }
		public Command AddCommand { get; }

		public ObservableCollection<TodoItem> Items
		{
			get { return items; }
			set { items = value; OnPropertyChanged(); }
		}

		public string Name
		{
			get { return name; }
			set { name = value; OnPropertyChanged(); }
		}

		public bool IsBusy
		{
			get { return isBusy; }
			set { isBusy = value; OnPropertyChanged(); }
		}


		async Task AddAsync()
		{
			IsBusy = true;

			var item = new TodoItem { Name = Name };
			try
			{
				await manager.SaveTaskAsync(item).ConfigureAwait(false);
				Items = await manager.GetTodoItemsAsync().ConfigureAwait(false);
				Name = string.Empty;
			}
			finally
			{
				IsBusy = false;
			}
		}


		async Task CompleteAsync(TodoItem item)
		{
			IsBusy = true;
			item.Done = true;

			try
			{
				await manager.SaveTaskAsync(item).ConfigureAwait(false);
				Items = await manager.GetTodoItemsAsync().ConfigureAwait(false);
			}
			finally
			{
				IsBusy = false;
			}
		}

		async Task RefreshAsync(bool? sync)
		{
			IsBusy = true;

			try
			{
				Items = await manager.GetTodoItemsAsync(sync.HasValue && sync.Value).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				await Application.Current.MainPage.DisplayAlert("Refresh Error", "Couldn't refresh data (" + ex.Message + ")", "OK");
			}
			finally
			{
				IsBusy = false;
			}
		}

		void OnPropertyChanged([CallerMemberName]string callerName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(callerName));
	}
}
