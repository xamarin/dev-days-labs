using System;

using Newtonsoft.Json;

namespace DevDaysTasks
{
	public class TodoItem
	{
		[JsonProperty(PropertyName = "id")]
		public string Id { get; set; }

		[JsonProperty(PropertyName = "text")]
		public string Name { get; set; }

		[JsonProperty(PropertyName = "complete")]
		public bool Done { get; set; }

		[Microsoft.WindowsAzure.MobileServices.Version]
		public string Version { get; set; }
	}
}

