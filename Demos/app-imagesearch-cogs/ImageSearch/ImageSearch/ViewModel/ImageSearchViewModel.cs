using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using MvvmHelpers;
using ImageSearch.Services;
using System.Net.Http;
using Newtonsoft.Json;
using ImageSearch.Model;
using Acr.UserDialogs;
using ImageSearch.Model.BingSearch;
using Plugin.Connectivity;

namespace ImageSearch.ViewModel
{
    public class ImageSearchViewModel
    {
        public ObservableRangeCollection<ImageResult> Images { get; }

        public ImageSearchViewModel()
        {
            Images = new ObservableRangeCollection<ImageResult>();
        }
        
        public async Task<bool> SearchForImagesAsync(string query)
        {

            if(!CrossConnectivity.Current.IsConnected)
            {
                await UserDialogs.Instance.AlertAsync("Not connected to the internet, please check connection.");
                return false;
            }

			//Bing Image API
			var url = $"https://api.cognitive.microsoft.com/bing/v5.0/images/" + 
				      $"search?q={WebUtility.UrlEncode(query)}" +
					  $"&count=20&offset=0&mkt=en-us&safeSearch=Strict";

            var requestHeaderKey = "Ocp-Apim-Subscription-Key";
            var requestHeaderValue = CognitiveServicesKeys.BingSearch;
            try
			{
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add(requestHeaderKey, requestHeaderValue);

                    var json = await client.GetStringAsync(url);

                    var result = JsonConvert.DeserializeObject<SearchResult>(json);

                    Images.ReplaceRange(result.Images.Select(i => new ImageResult
                    {
                        ContextLink = i.HostPageUrl,
                        FileFormat = i.EncodingFormat,
                        ImageLink = i.ContentUrl,
                        ThumbnailLink = i.ThumbnailUrl,
                        Title = i.Name
                    }));
                }
			}
			catch (Exception ex)
			{	
				await UserDialogs.Instance.AlertAsync("Unable to query images: " + ex.Message);
				return false;
			}

			
			return true;
        }
        
    }
}
