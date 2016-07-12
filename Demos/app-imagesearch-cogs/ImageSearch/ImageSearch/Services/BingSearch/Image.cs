using Newtonsoft.Json;

namespace ImageSearch.Model.BingSearch
{
	class Image
	{
		[JsonProperty("name")]
		public string Name { get; set; }
		[JsonProperty("webSearchUrl")]
		public string WebSearchUrl { get; set; }
		[JsonProperty("thumbnailUrl")]
		public string ThumbnailUrl { get; set; }
		[JsonProperty("datePublished")]
		public string DatePublished { get; set; }
		[JsonProperty("contentUrl")]
		public string ContentUrl { get; set; }
		[JsonProperty("hostPageUrl")]
		public string HostPageUrl { get; set; }
		[JsonProperty("contentSize")]
		public string ContentSize { get; set; }
		[JsonProperty("encodingFormat")]
		public string EncodingFormat { get; set; }
		[JsonProperty("hostPageDisplayUrl")]
		public string HostPageDisplayUrl { get; set; }
		[JsonProperty("width")]
		public int Width { get; set; }
		[JsonProperty("height")]
		public int Height { get; set; }
		[JsonProperty("thumbnail")]
		public Thumbnail Thumbnail { get; set; }
		[JsonProperty("imageInsightsToken")]
		public string ImageInsightsToken { get; set; }
		[JsonProperty("imageId")]
		public string ImageId { get; set; }
        [JsonProperty("accentColor")]
		public string AccentColor { get; set; }
	}
}
