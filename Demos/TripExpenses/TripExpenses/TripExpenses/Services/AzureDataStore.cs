
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.MobileServices;
using TripExpenses.Models;
using System.Linq;
using Microsoft.WindowsAzure.MobileServices.SQLiteStore;
using Microsoft.WindowsAzure.MobileServices.Sync;
using System.Diagnostics;
using System;
using Xamarin.Forms;
using TripExpenses.Services;

//Comment back in to use azure

//[assembly: Dependency(typeof(AzureDataStore))]
namespace TripExpenses.Services
{
  public class AzureDataStore : IDataStore
  {
    public List<TripExpense> Expenses { get; set; }
    public MobileServiceClient MobileService { get; set; }

    private IMobileServiceSyncTable<TripExpense> expenseTable;
    private bool initialized = false;

    public AzureDataStore()
    {

      Expenses = new List<TripExpense>();
      InitExpenses();
#if __ANDROID__ || __IOS__
      Microsoft.WindowsAzure.MobileServices.CurrentPlatform.Init();
#endif
#if __IOS__
     SQLitePCL.CurrentPlatform.Init();
#endif


      //comment back in to enable Azure Mobile Services.
      MobileService = new MobileServiceClient("https://" 
		+ "SITE-HERE" + ".azure-mobile.net/",
				"API-KEY-HERE");

      
    }

    public async Task Init()
    {
      initialized = true;
      string path = "syncstore.db";
      var store = new MobileServiceSQLiteStore(path);
      store.DefineTable<TripExpense>();
      await MobileService.SyncContext.InitializeAsync(store, new MobileServiceSyncHandler());

      expenseTable = MobileService.GetSyncTable<TripExpense>();
    }


		public async Task<TripExpense> InsertExpenseAsync(TripExpense expense)
    {
      if (!initialized)
        await Init();

      
      await expenseTable.InsertAsync(expense);
			return expense;
    }

		public async Task<TripExpense> UpdateExpenseAsync(TripExpense expense)
    {
      if (!initialized)
        await Init();
      
      await expenseTable.UpdateAsync(expense);
			return expense;
    }

		public async Task DeleteExpenseAsync(TripExpense expense)
		{
			if (!initialized)
				await Init();


			await expenseTable.DeleteAsync(expense);
		}

    public async Task<IEnumerable<TripExpense>> GetExpensesAsync()
    {
      if (!initialized)
        await Init();

      await SyncExpensesAsync();
      return await expenseTable.ToEnumerableAsync();
    }

    public async Task SyncExpensesAsync()
    {
      try
      {
        await MobileService.SyncContext.PushAsync();
				await expenseTable.PullAsync("allItems", expenseTable.CreateQuery());
      }
      catch(Exception ex)
      {
        Debug.WriteLine("Sync Failed:" + ex.Message);
      }
    }



    static readonly AzureDataStore instance = new AzureDataStore();
    /// <summary>
    /// Gets the instance of the Azure Web Service
    /// </summary>
    public static AzureDataStore Instance
    {
      get
      {
		return instance;
      }
    }





    private void InitExpenses()
    {
      Expenses.Add(new TripExpense
        {
          Billable = true,
          Category = "Transportation",
          Price = "1200.00",
          Id = "0",
          Name = "Flight to TechEd"
        });
      Expenses.Add(new TripExpense
      {
        Billable = true,
        Category = "Transportation",
        Price = "40.00",
        Id = "1",
        Name = "Taxi From Airport"
      });
      Expenses.Add(new TripExpense
      {
        Billable = true,
        Category = "Transportation",
        Price = "30.00",
        Id = "2",
        Name = "Taxi to Dinner"
      });
      Expenses.Add(new TripExpense
      {
        Billable = true,
        Category = "Meal",
        Price = "20.00",
        Id = "3",
        Name = "Meal night 1"
      });

      Expenses.Add(new TripExpense
      {
        Billable = true,
        Category = "Lodging",
        Price = "90.00",
        Id = "4",
        Name = "Hotel 1 Night"
      });
    }



 }
}