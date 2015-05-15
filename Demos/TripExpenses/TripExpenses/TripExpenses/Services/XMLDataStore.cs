using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Linq;
using PCLStorage;
using TripExpenses.Models;
using Xamarin.Forms;
using TripExpenses;

//Comment out to use azure
[assembly:Dependency(typeof(XmlDataStore))]
namespace TripExpenses
{
	public class XmlDataStore : IDataStore
	{

		public XmlDataStore()
		{
		}


		public static Task<T> DeserializeObjectAsync<T>(string value)
		{
			return Task.Factory.StartNew(() => JsonConvert.DeserializeObject<T>(value));
		}

		public static T DeserializeObject<T>(string value)
		{
			return JsonConvert.DeserializeObject<T>(value);
		}

		List<TripExpense> Expenses = new List<TripExpense>();

		public Task<TripExpense> GetExpenseAsync(string id)
		{
			return Task.Run(()=>Expenses.FirstOrDefault(s => s.Id == id));
		}

		public async Task<IEnumerable<TripExpense>> GetExpensesAsync()
		{
			var rootFolder = FileSystem.Current.LocalStorage;

			var folder = await rootFolder.CreateFolderAsync(Folder,
				CreationCollisionOption.OpenIfExists);

			var file = await folder.CreateFileAsync(File,
				CreationCollisionOption.OpenIfExists);

			var json = await file.ReadAllTextAsync();

			if(!string.IsNullOrWhiteSpace(json))
				Expenses = DeserializeObject<List<TripExpense>>(json);

			if(Expenses.Count == 0)
			{
				var expense =  new TripExpense
				{
					Billable = true,
					Category = "Transportation",
					Name = "DevWeek 2015 Flight",
					Price = "1000.00"
				};
				await SaveExpenseAsync(expense);
			}

			return Expenses;
		}

		public Task SyncExpensesAsync()
		{
			return Task.Run(() => { });
		}

		public async Task<TripExpense> InsertExpenseAsync(TripExpense expense)
		{
			return await SaveExpenseAsync (expense);
		}

		public async Task<TripExpense> UpdateExpenseAsync(TripExpense expense)
		{
			return await SaveExpenseAsync (expense);
		}

		public async Task<TripExpense> SaveExpenseAsync(TripExpense expense)
		{
			if(string.IsNullOrWhiteSpace(expense.Id))
			{
				expense.Id = DateTime.Now.ToString();
				Expenses.Add(expense);
			}
			else
			{
				var found = Expenses.FirstOrDefault(e => e.Id == expense.Id);
				if(found != null)
					found.SyncProperties(expense);
			}
			await Save();
			return expense;
		}

		public async Task DeleteExpenseAsync(TripExpense expense)
		{
			var id = expense.Id;
			Expenses.Remove(expense);
			await Save();
		}

		private string Folder = "Expenses";
		private string File = "expenses.json";

		private async Task Save()
		{
			var rootFolder = FileSystem.Current.LocalStorage;

			var folder = await rootFolder.CreateFolderAsync(Folder,
				CreationCollisionOption.OpenIfExists);

			var file = await folder.CreateFileAsync(File,
				CreationCollisionOption.ReplaceExisting);

			await file.WriteAllTextAsync(JsonConvert.SerializeObject(Expenses));
		}
			

		public Task Init()
		{
			return null;
		}
	}
}

