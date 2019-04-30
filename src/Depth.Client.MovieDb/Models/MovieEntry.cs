using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Depth.Client.MovieDb.Models
{
    public class MovieEntry
    {
        [JsonProperty("vote_count")]
        public int VoteCount { get; set; }

        public long Id { get; set; }

        [JsonProperty("video")]
        public bool HasVideo { get; set; }

        [JsonProperty("vote_average")]
        public float VoteAverage { get; set; }

        public string Title { get; set; }

        public float Popularity { get; set; }

        [JsonProperty("poster_path")]
        public string PosterPath { get; set; }

        [JsonProperty("original_language")]
        public string OriginalLanguage { get; set; }

        [JsonProperty("original_title")]
        public string OriginalTitle { get; set; }

        [JsonProperty("genre_ids")]
        public List<int> GenreIds { get; set; }

        [JsonProperty("backdrop_path")]
        public string BackdropPath { get; set; }

        [JsonProperty("adult")]
        public bool IsAdult { get; set; }

        public string Overview { get; set; }

        [JsonProperty("release_date")]
        public DateTime? ReleaseDate { get; set; }
    }
}
