using System;
using System.Net;
using System.Threading.Tasks;

using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;

namespace ImageSearch.Services
{
	static class ImageSearchServices
	{
		static readonly Lazy<ImageSearchAPI> imageSearchApiClient =
			new Lazy<ImageSearchAPI>(() => new ImageSearchAPI(new ApiKeyServiceClientCredentials(ServiceKeys.BingSearch)));

		#region Events
		public static event EventHandler InvalidApiKey;
		public static event EventHandler Error429_TooManyApiRequests;
		#endregion

		static ImageSearchAPI ImageSearchApiClient => imageSearchApiClient.Value;

		#region Methods
		public static async Task<Images> GetImage(string searchText)
		{
			try
			{
				return await ImageSearchApiClient.Images.SearchAsync(searchText).ConfigureAwait(false);
			}
			catch (ErrorResponseException e) when (e.Response.StatusCode.Equals(HttpStatusCode.Unauthorized))
			{
				OnInvalidApiKey();            
				throw;
			}
			catch (ErrorResponseException e) when (e.Response.StatusCode.Equals(429))
			{
				OnError429_TooManyApiRequests();            
				throw;
			}
			catch (ArgumentNullException)
			{
				OnInvalidApiKey();
				throw;
			}
		}

		static void OnInvalidApiKey() => InvalidApiKey?.Invoke(null, EventArgs.Empty);

		static void OnError429_TooManyApiRequests() => Error429_TooManyApiRequests?.Invoke(null, EventArgs.Empty);
		#endregion
	}
}
