using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TripExpenses.Models;
using TripExpenses.Services;
using Xamarin.Forms;

namespace TripExpenses.ViewModels
{
  public class DetailViewModel : BaseViewModel
  {
    private static List<string> categories = new List<string> { "Lodging", "Meal", "Transportation", "Other" };
    public static List<string> Categories
    {
      get { return categories; }
    }


    private TripExpense expense;
    private bool isNew;

    INavigation navigation;
		IDataStore dataStore;
    public DetailViewModel(TripExpense expense, INavigation navigation)
    {
			dataStore = DependencyService.Get<IDataStore> ();
      this.navigation = navigation;
      if(expense == null)
      {
        isNew = true;
        expense = new TripExpense()
        {
          Name = string.Empty,
          Category = Categories[2],
          Price = string.Empty
        };
      }
      this.expense = expense;
      name = expense.Name;
      category = Categories.IndexOf(expense.Category);
      billable = expense.Billable;
      price = expense.Price;
    }

    private Command saveCommand;
    public ICommand SaveCommand
    {
      get 
      { 
        return saveCommand ??
        (saveCommand = new Command(async () => await ExecuteSaveCommand()));
      }
    }

    private async Task ExecuteSaveCommand()
    {
      if (IsBusy)
        return;

      IsBusy = true;

      expense.Name = string.IsNullOrWhiteSpace(name) ? "New Expense" : name;
      expense.Billable = billable;
      expense.Category = Categories[category];
      expense.Price = string.IsNullOrWhiteSpace(price) ? "0.00" : price;


      if (isNew)
				await dataStore.InsertExpenseAsync(expense);
      else
				await dataStore.UpdateExpenseAsync(expense);


			await dataStore.SyncExpensesAsync();

      //Send a message to insert/update the expense to all subscribers
      if(isNew)
      {
        MessagingCenter.Send(expense, "NewExpense");
      }
      else
      {
        MessagingCenter.Send(expense, "UpdateExpense");
      }
      IsBusy = false;
      navigation.PopAsync();
    }


    private string name = string.Empty;
    public string Name
    {
      get { return name; }
      set { name = value; OnPropertyChanged("Name"); }
    }

    private int category = 0;
    public int Category
    {
      get { return category; }
      set { category = value; OnPropertyChanged("Category"); }
    }


    private bool billable;
    public bool Billable
    {
      get { return billable; }
      set { billable = value; OnPropertyChanged("Billable"); }
    }

    private string price;
    public string Price
    {
      get { return price; }
      set { price = value; OnPropertyChanged("Price"); }
    }
  }
}
