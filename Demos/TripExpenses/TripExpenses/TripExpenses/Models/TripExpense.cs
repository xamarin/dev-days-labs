using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace TripExpenses.Models
{
  public class TripExpense
  {
    [JsonProperty(PropertyName = "id")]
    public string Id { get; set; }

    public string Name { get; set; }
    public bool Billable { get; set; }
    public string Category { get; set; }
    public string Price { get; set; }


    [Microsoft.WindowsAzure.MobileServices.Version]
    public string Version { get; set; }  


  }
}
