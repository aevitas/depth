using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Depth.Client.YouTube.Models;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Options;

namespace Depth.Client.YouTube
{
    public class YouTubeClient
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
            var videos = response.Items.Where(i => i.Kind.Equals("youtube#video", StringComparison.OrdinalIgnoreCase));

            return videos.Select(v => new VideoEntry
            {
                Description = v.Snippet.Description,
                Title = v.Snippet.Title,
                PublishedAt = v.Snippet.PublishedAt
            });
        }
    }
}
