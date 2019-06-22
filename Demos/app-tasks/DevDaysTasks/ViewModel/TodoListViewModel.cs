using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices;
using AsyncAwaitBestPractices.MVVM;
using Xamarin.Forms;

namespace DevDaysTasks
{
    public class TodoListViewModel : INotifyPropertyChanged
    {
        readonly WeakEventManager onPropertyChangedEventManager = new WeakEventManager();
        readonly AzureService manager;

        string name;
        bool isBusy;

        public TodoListViewModel()
        {
            manager = AzureService.DefaultManager;
            Items = new ObservableCollection<TodoItem>();

            AddCommand = new AsyncCommand(AddAsync, continueOnCapturedContext: false);
            CompleteCommand = new AsyncCommand<TodoItem>(item => CompleteAsync(item), continueOnCapturedContext: false);
            SyncCommand = new AsyncCommand(() => RefreshAsync(true), continueOnCapturedContext: false);
            RefreshCommand = new AsyncCommand(() => RefreshAsync(false), continueOnCapturedContext: false);
        }

        event PropertyChangedEventHandler INotifyPropertyChanged.PropertyChanged
        {
            add => onPropertyChangedEventManager.AddEventHandler(value);
            remove => onPropertyChangedEventManager.RemoveEventHandler(value);
        }

        public ICommand CompleteCommand { get; }
        public ICommand SyncCommand { get; }
        public ICommand RefreshCommand { get; }
        public ICommand AddCommand { get; }

        public ObservableCollection<TodoItem> Items { get; } = new ObservableCollection<TodoItem>();

        public string Name
        {
            get => name;
            set => SetProperty(ref name, value);
        }

        public bool IsBusy
        {
            get => isBusy;
            set => SetProperty(ref isBusy, value);
        }


        async Task AddAsync()
        {
            IsBusy = true;

            var item = new TodoItem { Name = Name };
            try
            {
                await manager.SaveTaskAsync(item).ConfigureAwait(false);
                var items = await manager.GetTodoItemsAsync().ConfigureAwait(false);

                ReplaceItemsInList(items);

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
                var items = await manager.GetTodoItemsAsync().ConfigureAwait(false);

                ReplaceItemsInList(items);
            }
            finally
            {
                IsBusy = false;
            }
        }

        async Task RefreshAsync(bool? sync)
        {
            try
            {
                var todoItems = await manager.GetTodoItemsAsync(sync.HasValue && sync.Value).ConfigureAwait(false);

                ReplaceItemsInList(todoItems);
            }

            catch (Exception ex)
            {
                Device.BeginInvokeOnMainThread(async () => await Application.Current.MainPage.DisplayAlert("Refresh Error", "Couldn't refresh data (" + ex.Message + ")", "OK"));
            }
            finally
            {
                IsBusy = false;
            }
        }

        void ReplaceItemsInList(IEnumerable<TodoItem> todoItems)
        {
            Items.Clear();

            foreach (var item in todoItems)
                Items.Add(item);
        }

        void SetProperty<T>(ref T backingStore, T value, Action onChanged = null, [CallerMemberName] string propertyname = "")
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return;

            backingStore = value;

            onChanged?.Invoke();

            OnPropertyChanged(propertyname);
        }

        void OnPropertyChanged([CallerMemberName]string callerName = "") =>
            onPropertyChangedEventManager.HandleEvent(this, new PropertyChangedEventArgs(callerName), nameof(INotifyPropertyChanged.PropertyChanged));
    }
}
