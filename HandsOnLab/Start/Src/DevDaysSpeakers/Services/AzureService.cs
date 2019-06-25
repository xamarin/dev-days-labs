using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

using DevDaysSpeakers.Shared.Models;

using Newtonsoft.Json;

namespace DevDaysSpeakers.Services
{
    public static class AzureService
    {
     	//Change this to match your Azure Functions URL	
        const string getSpeakersFunctionUrl = "[Your Azure Functions URL]";

		static readonly Lazy<HttpClient> clientHolder = new Lazy<HttpClient>();
		static readonly Lazy<JsonSerializer> serializer = new Lazy<JsonSerializer>();

		static HttpClient Client => clientHolder.Value;
		static JsonSerializer Serializer => serializer.Value;

		public static Task<List<Speaker>> GetSpeakers() => GetObjectFromAPI<List<Speaker>>(getSpeakersFunctionUrl);

		static async Task<T> GetObjectFromAPI<T>(string apiUrl)
		{
			using (var stream = await Client.GetStreamAsync(apiUrl))
			using (var streamReader = new StreamReader(stream))
			using (var json = new JsonTextReader(streamReader))
			{
				return Serializer.Deserialize<T>(json);
			}
		}
	}
}
