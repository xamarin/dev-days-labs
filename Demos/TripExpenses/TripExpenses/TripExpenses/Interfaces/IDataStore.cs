using System;
using System.Threading.Tasks;
using TripExpenses.Models;
using System.Collections.Generic;

namespace TripExpenses
{
	public interface IDataStore
	{
		Task Init();
		Task<TripExpense> InsertExpenseAsync(TripExpense expense);
		Task<TripExpense> UpdateExpenseAsync(TripExpense expense);
		Task<IEnumerable<TripExpense>> GetExpensesAsync();
		Task SyncExpensesAsync();
		Task DeleteExpenseAsync(TripExpense expense);
	}
}

