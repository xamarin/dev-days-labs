using Newtonsoft.Json;

namespace ImageSearch.Model.BingSearch
{
	class Suggestion
	{
		[JsonProperty("text")]
		public string Text { get; set; }
		[JsonProperty("displayText")]
		public string DisplayText { get; set; }
		[JsonProperty("webSearchUrl")]
		public string WebSearchUrl { get; set; }
		[JsonProperty("searchLink")]
		public string SearchLink { get; set; }
		[JsonProperty("thumbnail")]
		public Thumbnail Thumbnail { get; set; }
	}
}
