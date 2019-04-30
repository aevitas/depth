using System.Collections.Generic;
using Newtonsoft.Json;

namespace Depth.Client.MovieDb.Models
{
    internal class PaginatedSearchResult
    {
        [JsonProperty("page")]
        public int Page { get; set; }

        [JsonProperty("total_results")]
        public int TotalResultCount { get; set; }

        [JsonProperty("total_pages")]
        public int TotalPageCount { get; set; }

        [JsonProperty("results")]
        public IEnumerable<MovieEntry> Results { get; set; }
    }
}
