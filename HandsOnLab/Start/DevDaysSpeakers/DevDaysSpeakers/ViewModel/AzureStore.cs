using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.WindowsAzure.MobileServices;
using Microsoft.WindowsAzure.MobileServices.Sync;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using DevDaysSpeakers.Model;
using System.Diagnostics;

namespace DevDaysSpeakers.ViewModel
{
    public class AzureStore
    {
        static AzureStore current;
        public static AzureStore Current
        {
            get
            {
                if (current == null)
                    current = new AzureStore();

                return current;
            }
        }

        MobileServiceClient MobileService { get; set; }
        IMobileServiceSyncTable<Speaker> speakerTable;

        public async Task Initialize()
        {
            if (MobileService != null)
                return;

            MobileService = new MobileServiceClient("https://montemagnospeakers.azurewebsites.net");

            var store = new MobileServiceSQLiteStore("speakers.db");

            store.DefineTable<Speaker>();

            await MobileService.SyncContext.InitializeAsync(store);

            speakerTable = MobileService.GetSyncTable<Speaker>();
        }

        public async Task<List<Speaker>> GetSpeakers()
        {
            await Initialize();

            try
            {
                await speakerTable.PullAsync("allSpeakers", speakerTable.CreateQuery());
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }

            return await speakerTable.ToListAsync();
        }

    }
}
