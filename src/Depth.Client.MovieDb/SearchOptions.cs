using Newtonsoft.Json;

namespace Depth.Client.MovieDb
{
    public sealed class SearchOptions
    {
        public string Query { get; set; }

        public string Language { get; set; }

        public int? Page { get; set; }

        [JsonProperty("include_adult")]
        public bool IncludeAdult { get; set; }

        public string Region { get; set; }

        public int? Year { get; set; }

        [JsonProperty("primary_release_year")]
        public int? PrimaryReleaseYear { get; set; }
    }
}
