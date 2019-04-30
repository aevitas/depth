using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Depth.Client.MovieDb.Models
{
    public class MovieDetail
    {
        [JsonProperty("adult")]
        public bool? IsAdult { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("budget")]
        public long? Budget { get; set; }

        [JsonProperty("genres")]
        public List<KeyValuePair<int, string>> Genres { get; set; }

        [JsonProperty("homepage")]
        public string Homepage { get; set; }

        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("overview")]
        public string Description { get; set; }

        [JsonProperty("popularity")]
        public float? Popularity { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("release_date")]
        public DateTimeOffset ReleaseDate { get; set; }

        [JsonProperty("revenue")]
        public long Revenue { get; set; }

        [JsonProperty("runtime")]
        public int? Runtime { get; set; }
        
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("tagline")]
        public string Tagline { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("vote_average")]
        public float? Rating { get; set; }

        [JsonProperty("vote_count")]
        public int? VoteCount { get; set; }
    }
}
