using System;
using System.Threading.Tasks;
using Depth.Client.MovieDb.Abstractions;
using Depth.Client.MovieDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Caching.Memory;

namespace Depth.Api.Controllers
{
    [Route("movies")]
    public class MovieController : Controller
    {
        private readonly IMovieDetailProvider _detailProvider;
        private readonly IMovieSearchProvider _searchProvider;
        private readonly IMemoryCache _memoryCache;

        public MovieController(IMovieSearchProvider searchProvider, IMovieDetailProvider detailProvider,
            IMemoryCache memoryCache)

        {
            _searchProvider = searchProvider ?? throw new ArgumentNullException(nameof(searchProvider));
            _detailProvider = detailProvider ?? throw new ArgumentNullException(nameof(detailProvider));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
        }

        [HttpGet("search")]
        public async Task<ActionResult<MovieEntry>> Search(string query, bool includeAdult = false)
        {
            var key = GetSearchCacheKey(query);

            if (_memoryCache.TryGetValue(key, out var cachedValue))
                return Ok(cachedValue);

            var result = await _searchProvider.SearchAsync(opts =>
            {
                opts.Query = query;
                opts.IncludeAdult = includeAdult;
            });

            if (result == null || !result.Any())
                return NotFound();

            _memoryCache.Set(key, result);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDetail>> Detail(int id)
        {
            var key = GetDetailCacheKey(id);

            if (_memoryCache.TryGetValue(key, out var cachedValue))
                return Ok(cachedValue);

            var result = await _detailProvider.GetDetailAsync(id);

            if (result == null)
                return NotFound();

            _memoryCache.Set(key, result);

            return Ok(result);
        }

        private static string GetSearchCacheKey(string query) => $"movie.search.{query}";
        private static string GetDetailCacheKey(int id) => $"movie.detail.{id}";
    }
}