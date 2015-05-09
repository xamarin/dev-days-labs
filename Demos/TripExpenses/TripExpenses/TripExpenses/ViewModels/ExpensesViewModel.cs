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

    public ExpensesViewModel()
    { 
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

      var expenses = await DataStore.Instance.GetExpensesAsync();
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
  }
}
