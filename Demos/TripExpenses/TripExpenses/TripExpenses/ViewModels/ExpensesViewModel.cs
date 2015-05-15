using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TripExpenses.Models;
using TripExpenses.Services;
using Xamarin.Forms;

namespace TripExpenses.ViewModels
{
  public class ExpensesViewModel : BaseViewModel
  {

    public bool Initialized { get; set; }

    public ObservableCollection<TripExpense> Expenses { get; set; }
		IDataStore dataStore;

    public ExpensesViewModel()
		{ 
			dataStore = DependencyService.Get<IDataStore> ();
      Expenses = new ObservableCollection<TripExpense>();
      //Subscibe to insert expenses
      MessagingCenter.Subscribe<TripExpense>(this, "NewExpense", (expense) =>
      {
        Expenses.Add(expense);
      });

      //subscribe to update expenxes
      MessagingCenter.Subscribe<TripExpense>(this, "UpdateExpense", (expense) =>
      {
        ExecuteUpdateExpense(expense);
      });
    }


    private Command loadExpenses;
    public ICommand LoadExpenses
    {
      get 
      { 
        return loadExpenses ??
        (loadExpenses = new Command(async () => await ExecuteLoadExpenses()));
      }
    }

    private async Task ExecuteLoadExpenses()
    {
      if (IsBusy)
        return;

      Initialized = true;
      IsBusy = true;

      Expenses.Clear();

			var expenses = await dataStore.GetExpensesAsync();
      foreach (var expense in expenses)
       Expenses.Add(expense);

      IsBusy = false;
    }


    private Command<TripExpense> updateExpense;
    public ICommand UpdateExpense
    {
      get
      {
        return updateExpense ??
        (updateExpense = new Command<TripExpense>(ExecuteUpdateExpense));
      }
    }

    private void ExecuteUpdateExpense(TripExpense expense)
    {
      IsBusy = true;
      var index = Expenses.IndexOf(expense);
      Expenses[index] = expense;
      IsBusy = false;
    }

		private Command<TripExpense> deleteExpense;
		public ICommand DeleteExpense
		{
			get
			{
				return deleteExpense ??
					(deleteExpense = new Command<TripExpense>((expense)=>ExecuteDeleteExpense(expense)));
			}
		}

		private async Task ExecuteDeleteExpense(TripExpense expense)
		{
			IsBusy = true;
			await dataStore.DeleteExpenseAsync(expense);
			await dataStore.SyncExpensesAsync ();
			Expenses.Remove (expense);
			IsBusy = false;
		}


		public void SpeakTotal()
		{
			var total = 0.0f;
			var temp = 0.0f;
			foreach (var expense in Expenses) {
				
				if (float.TryParse (expense.Price, out temp))
					total += temp;
			}

			var message = "You have a total of " + 
				Expenses.Count + 
				" expenses that need to be filed totaling " +
				total.ToString("C");
	


		}
  }
}
