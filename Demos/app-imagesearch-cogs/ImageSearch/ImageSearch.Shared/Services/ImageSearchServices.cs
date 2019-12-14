using System;
using System.Net;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;

namespace ImageSearch.Services
{
    static class ImageSearchServices
    {
        static readonly WeakEventManager _invalidApiKeyEventManager = new WeakEventManager();
        static readonly WeakEventManager _error429TooManyApiRequestsEventManager = new WeakEventManager();

        static readonly Lazy<ImageSearchClient> imageSearchApiClient =
            new Lazy<ImageSearchClient>(() => new ImageSearchClient(new ApiKeyServiceClientCredentials(ServiceKeys.BingSearch)));

        public static event EventHandler InvalidApiKey
        {
            add => _invalidApiKeyEventManager.AddEventHandler(value);
            remove => _invalidApiKeyEventManager.RemoveEventHandler(value);
        }

        public static event EventHandler Error429_TooManyApiRequests
        {
            add => _error429TooManyApiRequestsEventManager.AddEventHandler(value);
            remove => _error429TooManyApiRequestsEventManager.RemoveEventHandler(value);
        }

        static ImageSearchClient ImageSearchApiClient => imageSearchApiClient.Value;

        public static async Task<Images> GetImage(string searchText)
        {
            try
            {
                return await ImageSearchApiClient.Images.SearchAsync(searchText).ConfigureAwait(false);
            }
            catch (ErrorResponseException e) when (e.Response.StatusCode is HttpStatusCode.Unauthorized)
            {
                OnInvalidApiKey();
                throw;
            }
            catch (ErrorResponseException e) when (e.Response.StatusCode is HttpStatusCode.TooManyRequests)
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

        static void OnInvalidApiKey() => _invalidApiKeyEventManager.HandleEvent(null, EventArgs.Empty, nameof(InvalidApiKey));
        static void OnError429_TooManyApiRequests() => _error429TooManyApiRequestsEventManager.HandleEvent(null, EventArgs.Empty, nameof(Error429_TooManyApiRequests));
    }
}
