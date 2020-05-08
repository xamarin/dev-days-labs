using System;
using System.Linq;
using System.Threading.Tasks;
using Acr.UserDialogs;
using ImageSearch.Services;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;
using MvvmHelpers;
using Xamarin.Essentials;

namespace ImageSearch.ViewModel
{
    public class ImageSearchViewModel
    {
        public ObservableRangeCollection<ImageObject?> Images { get; } = new ObservableRangeCollection<ImageObject?>();

        public async Task<bool> SearchForImagesAsync(string query)
        {
            if (Connectivity.NetworkAccess is NetworkAccess.Internet)
            {
                try
                {
                    Images? images = await ImageSearchServices.GetImage(query).ConfigureAwait(false);
                    var filteredImages = images?.Value.Where(x => x?.ContentUrl?.Contains("https") ?? false) ?? Enumerable.Empty<ImageObject>();

                    Images.Clear();
                    Images.AddRange(filteredImages);

                    return true;
                }
                catch (Exception ex)
                {
                    await UserDialogs.Instance.AlertAsync("Unable to query images: " + ex.Message);
                    return false;
                }
            }
            else
            {
                await UserDialogs.Instance.AlertAsync("Not connected to the internet, please check connection.");
                return false;
            }
        }
    }
}
