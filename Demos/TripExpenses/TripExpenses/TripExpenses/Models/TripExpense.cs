using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TripExpenses.Models
{
  public class TripExpense
  {
		public TripExpense()
		{
		}

    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    public string Name { get; set; }
    public bool Billable { get; set; }
    public string Category { get; set; }
    public string Price { get; set; }


    [Microsoft.WindowsAzure.MobileServices.Version]
    public string Version { get; set; }  

		public void SyncProperties(TripExpense expense)
		{
			this.Billable = expense.Billable;
			this.Category = expense.Category;
			this.Name = expense.Name;
			this.Price = expense.Price;
		}
  }
}
