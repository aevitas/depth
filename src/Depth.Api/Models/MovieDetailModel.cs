using Depth.Client.MovieDb.Models;
using Newtonsoft.Json;

namespace Depth.Api.Models
{
    public class MovieDetailModel
    {
        [JsonProperty("movie")]
        public MovieDetail Movie { get; set; }

        [JsonProperty("trailer")]
        public TrailerModel Trailer { get; set; }

        [JsonProperty("has_trailer")]
        public bool HasTrailer => Trailer != null;
    }
}
