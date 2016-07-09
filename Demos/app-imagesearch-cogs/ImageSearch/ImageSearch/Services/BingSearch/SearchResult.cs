using Newtonsoft.Json;
using System.Collections.Generic;

namespace ImageSearch.Model.BingSearch
{
	class SearchResult
	{
		[JsonProperty("readLink")]
		public string ReadLink { get; set; }
		[JsonProperty("webSearchUrl")]
		public string WebSearchUrl { get; set; }
		[JsonProperty("totalEstimatedMatches")]
		public int TotalEstimatedMatches { get; set; }
		[JsonProperty("value")]
		public List<Image> Images { get; set; }
		[JsonProperty("nextOffsetAddCount")]
		public int NextOffsetAddCount { get; set; }
		[JsonProperty("displayShoppingSourcesBadges")]
		public bool DisplayShoppingSourcesBadges { get; set; }
		[JsonProperty("displayRecipeSourcesBadges")]
		public bool DisplayRecipeSourcesBadges { get; set; }
	}
}
