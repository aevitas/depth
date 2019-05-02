using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Depth.Client.YouTube.Abstractions;
using Depth.Client.YouTube.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
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
        public async Task<ActionResult<IEnumerable<VideoEntry>>> Search(string query)
        {
            var key = GetSearchCacheKey(query);
            if (_memoryCache.TryGetValue(key, out var cachedValue))
                return Ok(cachedValue);

            var result = await _searchProvider.SearchAsync(query);

            if (!result.Any())
                return NotFound();

            _memoryCache.Set(key, result);

            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<VideoEntry>> Trailer(string movie)
        {
            var key = GetTrailerCacheKey(movie);
            if (_memoryCache.TryGetValue(key, out var cachedValue))
                return Ok(cachedValue);

            var result = await _trailerProvider.GetTrailerAsync(movie);

            if (result == null)
                return NotFound();

            _memoryCache.Set(key, result);

            return Ok(result);
        }

        private static string GetSearchCacheKey(string query) => $"video.search.{query}";
        private static string GetTrailerCacheKey(string movie) => $"video.trailer.{movie}";
    }
}
