
using System;
using System.IO;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;

using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

namespace DevDaysTasks
{
    public partial class TodoItemManager
    {
        MobileServiceClient client;
        IMobileServiceSyncTable<TodoItem> todoTable;

        // Singleton instance
        private static TodoItemManager defaultManager;
        public static TodoItemManager DefaultManager
        {
            get { return defaultManager ?? (defaultManager = new TodoItemManager()); }
        }

        public TodoItemManager()
        {
            defaultManager = this;
        }

        public async Task Initialize()
        {
            // Check if Sync Context already has been synchronized
            if (client?.SyncContext?.IsInitialized ?? false)
                return;

            // Initialize Mobile Client
            client = new MobileServiceClient(Constants.ApplicationURL);

            // Initialize local database for syncing 
            var path = Path.Combine(MobileServiceClient.DefaultDatabasePath, Constants.SyncStorePath);
            var store = new MobileServiceSQLiteStore(path);
            store.DefineTable<TodoItem>();

            //Initializes the SyncContext using the default IMobileServiceSyncHandler.
            var handler = new MobileServiceSyncHandler();
            await client.SyncContext.InitializeAsync(store, handler);

            todoTable = client.GetSyncTable<TodoItem>();
        }

        public async Task<ObservableCollection<TodoItem>> GetTodoItemsAsync(bool syncItems = false)
        {
            try
            {
                // Make sure, the manager has been initialized
                await Initialize();

                // Check if synchronization with backend is requested
                if (syncItems)
                {
                    await SyncAsync();
                }
                // Get all uncompleted items from the local database
                var items = await todoTable
                    .Where(todoItem => !todoItem.Done)
                    .OrderBy(todoItem => todoItem.Name)
                    .ToEnumerableAsync();

                return new ObservableCollection<TodoItem>(items);
            }
            catch (MobileServiceInvalidOperationException msioe)
            {
                Debug.WriteLine(@"Invalid sync operation: {0}", msioe.Message);
                throw;
            }
            catch (Exception e)
            {
                Debug.WriteLine(@"Sync error: {0}", e.Message);
                throw;
            }
        }

        public async Task SaveTaskAsync(TodoItem item)
        {
            // Make sure, the manager has been initialized
            await Initialize();

            // Check if item is new or has already been existent by checking its Id
            if (item.Id == null)
            {
                // Insert new item
                await todoTable.InsertAsync(item);
            }
            else
            {
                // Update existing item
                await todoTable.UpdateAsync(item);
            }            
        }


        public async Task SyncAsync()
        {
            ReadOnlyCollection<MobileServiceTableOperationError> syncErrors = null;

            // Make sure, the manager has been initialized
            await Initialize();

            try
            {
                await client.SyncContext.PushAsync();

                //The first parameter is a query name that is used internally by the client SDK to implement incremental sync.
                //Use a different query name for each unique query in your program
                await todoTable.PullAsync("allTodoItems", todoTable.CreateQuery());
            }
            catch (MobileServicePushFailedException exc)
            {
                if (exc.PushResult != null)
                {
                    syncErrors = exc.PushResult.Errors;
                }
            }

            // Simple error/conflict handling. A real application would handle the various errors like network conditions,
            // server conflicts and others via the IMobileServiceSyncHandler.
            if (syncErrors != null)
            {
                foreach (var error in syncErrors)
                {
                    if (error.OperationKind == MobileServiceTableOperationKind.Update && error.Result != null)
                    {
                        //Update failed, reverting to server's copy.
                        await error.CancelAndUpdateItemAsync(error.Result);
                    }
                    else if(error.OperationKind != MobileServiceTableOperationKind.Update)
                    {
                        // Discard local change.
                        await error.CancelAndDiscardItemAsync();
                    }

                    Debug.WriteLine(@"Error executing sync operation. Item: {0} ({1}). Operation discarded.", error.TableName, error.Item["id"]);
                }
            }
        }
    }
}
