using Newtonsoft.Json;

namespace Depth.Client.MovieDb
{
    public sealed class MovieQueryOptions
    {
        public string Query { get; set; }

        [JsonProperty("include_adult")]
        public bool IncludeAdult { get; set; }
    }
}
