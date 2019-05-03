using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Depth.Api.Extensions;
using Depth.Api.Models;
using Depth.Client.YouTube.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Depth.Api.Controllers
{
    [Route("videos")]
    public class VideoController : Controller
    {
        private readonly IVideoSearchProvider _searchProvider;
        private readonly IMovieTrailerProvider _trailerProvider;
        private readonly IMemoryCache _memoryCache;

        public VideoController(IVideoSearchProvider videoSearchProvider, IMovieTrailerProvider movieTrailerProvider, IMemoryCache memoryCache)
        {
            _searchProvider = videoSearchProvider ?? throw new ArgumentNullException(nameof(videoSearchProvider));
            _trailerProvider = movieTrailerProvider ?? throw new ArgumentNullException(nameof(movieTrailerProvider));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<TrailerModel>>> Search(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return NotFound();

            var key = GetSearchCacheKey(query);
            if (_memoryCache.TryGetValue(key, out var cachedValue))
                return Ok(cachedValue);

            var result = await _searchProvider.SearchAsync(query);

            if (!result.Any())
                return NotFound();

            var models = result.Select(r => r.ToTrailerModel());

            _memoryCache.Set(key, models);

            return Ok(models);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<TrailerModel>> Trailer(string movie)
        {
            if (string.IsNullOrWhiteSpace(movie))
                return NotFound();

            var key = GetTrailerCacheKey(movie);
            if (_memoryCache.TryGetValue(key, out var cachedValue))
                return Ok(cachedValue);

            var result = await _trailerProvider.GetTrailerAsync(movie);

            if (result == null)
                return NotFound();

            var model = result.ToTrailerModel();

            _memoryCache.Set(key, model);

            return Ok(model);
        }

        private static string GetSearchCacheKey(string query) => $"video.search.{query}";
        private static string GetTrailerCacheKey(string movie) => $"video.trailer.{movie}";
    }
}
