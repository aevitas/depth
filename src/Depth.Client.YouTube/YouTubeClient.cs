using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Depth.Client.YouTube.Abstractions;
using Depth.Client.YouTube.Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Options;

namespace Depth.Client.YouTube
{
    public class YouTubeClient : IVideoSearchProvider, IMovieTrailerProvider
    {
        private readonly YouTubeService _service;

        public YouTubeClient(IOptions<YouTubeOptions> options)
        {
            var opts = options?.Value ?? throw new ArgumentNullException(nameof(options));

            _service = new YouTubeService(new BaseClientService.Initializer
            {
                ApiKey = opts.ApiKey
            });
        }

        public async Task<IEnumerable<VideoEntry>> SearchAsync(string query, int maximumResultCount = 10)
        {
            if (string.IsNullOrWhiteSpace(query))
                throw new ArgumentNullException(nameof(query));

            var request = _service.Search.List("snippet");

            request.Q = query;
            request.MaxResults = maximumResultCount;

            var response = await request.ExecuteAsync();
            var videos = response.Items.Where(i => i.Id.Kind.Equals("youtube#video", StringComparison.OrdinalIgnoreCase));

            return videos.Select(v => new VideoEntry
            {
                Description = v.Snippet.Description,
                Title = v.Snippet.Title,
                PublishedAt = v.Snippet.PublishedAt,
                Id = v.Id.VideoId
            });
        }

        public async Task<VideoEntry> GetTrailerAsync(string movie)
        {
            if (string.IsNullOrWhiteSpace(movie))
                throw new ArgumentNullException(nameof(movie));

            // Alright so, if the movie already contains the "trailer" keyword, we'll just use that for the search.
            // Otherwise, we're going to do a little manipulation so it contains trailer at the end.
            var containsTrailer = movie.IndexOf("trailer", StringComparison.OrdinalIgnoreCase) > 0;
            var query = containsTrailer ? movie : movie.TrimEnd(' ') + " trailer";

            var result = await SearchAsync(query, 1);

            return result.FirstOrDefault();
        }
    }
}
