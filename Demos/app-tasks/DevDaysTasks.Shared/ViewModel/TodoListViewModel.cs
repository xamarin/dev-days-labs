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
        private TodoItemManager manager;

        private ObservableCollection<TodoItem> items;
        public ObservableCollection<TodoItem> Items
        {
            get { return items; }
            set { items = value; OnPropertyChanged(); }
        }

        private string name;
        public string Name
        {
            get { return name; }
            set { name = value; OnPropertyChanged(); }
        }

        private bool isBusy;
        public bool IsBusy
        {
            get { return isBusy; }
            set { isBusy = value; OnPropertyChanged(); }
        }

        public TodoListViewModel()
        {
            manager = TodoItemManager.DefaultManager;
            Items = new ObservableCollection<TodoItem>();

            AddCommand = new Command(async () => await AddAsync());
            CompleteCommand = new Command<TodoItem>(async (item) => await CompleteAsync(item));
            SyncCommand = new Command(async () => await RefreshAsync(true));
            RefreshCommand = new Command(async () => await RefreshAsync(false));
        }       

        public Command AddCommand { get; }
        public async Task AddAsync()
        {
            IsBusy = true;
            var item = new TodoItem { Name = Name };
            await manager.SaveTaskAsync(item);
            Items = await manager.GetTodoItemsAsync();
            Name = string.Empty;
            IsBusy = false;
        }

        public Command<TodoItem> CompleteCommand { get; }
        public async Task CompleteAsync(TodoItem item)
        {
            IsBusy = true;
            item.Done = true;
            await manager.SaveTaskAsync(item);
            Items = await manager.GetTodoItemsAsync();
            IsBusy = false;
        }


        public Command SyncCommand { get; } 
        public Command RefreshCommand { get; }
        async Task RefreshAsync(bool? sync)
        {
            IsBusy = true;
            try
            {
                Items = await manager.GetTodoItemsAsync(sync.HasValue && sync.Value);
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

        // Implement INotifyPropteryChanged
        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName]string name = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
