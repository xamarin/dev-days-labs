using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using DevDaysSpeakers.Model;
using DevDaysSpeakers.Services;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;

using Xamarin.Forms;

[assembly: Dependency(typeof(AzureService))]
namespace DevDaysSpeakers.Services
{
    public class AzureService
    {
        IMobileServiceSyncTable<Speaker> table;

        public MobileServiceClient Client { get; set; } = null;

        public async Task Initialize()
        {
            if (Client?.SyncContext?.IsInitialized ?? false)
                return;

            var appUrl = "https://minnickspeakers.azurewebsites.net";

            //Create our client
            Client = new MobileServiceClient(appUrl);

            //InitialzeDatabase for path
            var path = "syncstore.db";
            path = Path.Combine(MobileServiceClient.DefaultDatabasePath, path);


            //setup our local sqlite store and intialize our table
            var store = new MobileServiceSQLiteStore(path);

            //Define table
            store.DefineTable<Speaker>();

            //Initialize SyncContext
            await Client.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

            //Get our sync table that will call out to azure
            table = Client.GetSyncTable<Speaker>();
        }

        public async Task<IEnumerable<Speaker>> GetSpeakers()
        {
            await Initialize();
            await SyncSpeakers();

            return await table.OrderBy(s => s.Name).ToEnumerableAsync();
        }

        public async Task SyncSpeakers()
        {
            try
            {
                await Client.SyncContext.PushAsync();
                await table.PullAsync("allSpeakers", table.CreateQuery());
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Unable to sync speakers, that is alright as we have offline capabilities: " + ex);
            }
        }
    }
}