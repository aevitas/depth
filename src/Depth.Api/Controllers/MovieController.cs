using System;
using System.Threading.Tasks;
using Depth.Api.Extensions;
using Depth.Api.Models;
using Depth.Client.MovieDb.Abstractions;
using Depth.Client.MovieDb.Models;
using Depth.Client.YouTube.Abstractions;
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
        private readonly IMovieTrailerProvider _trailerProvider;
        private readonly IMemoryCache _memoryCache;

        public MovieController(IMovieSearchProvider searchProvider, IMovieDetailProvider detailProvider, IMovieTrailerProvider trailerProvider,
            IMemoryCache memoryCache)
        {
            _searchProvider = searchProvider ?? throw new ArgumentNullException(nameof(searchProvider));
            _detailProvider = detailProvider ?? throw new ArgumentNullException(nameof(detailProvider));
            _trailerProvider = trailerProvider ?? throw new ArgumentNullException(nameof(trailerProvider));
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
        public async Task<ActionResult<MovieDetailModel>> Detail(int id)
        {
            var key = GetDetailCacheKey(id);

            if (_memoryCache.TryGetValue(key, out var cachedValue))
                return Ok(cachedValue);

            var movie = await _detailProvider.GetDetailAsync(id);

            if (movie == null)
                return NotFound();

            var trailer = await _trailerProvider.GetTrailerAsync(movie.Title);

            var model = new MovieDetailModel
            {
                Movie = movie
            };

            if (trailer != null)
                model.Trailer = trailer.ToTrailerModel();
            
            _memoryCache.Set(key, model);

            return Ok(model);
        }

        private static string GetSearchCacheKey(string query) => $"movie.search.{query}";
        private static string GetDetailCacheKey(int id) => $"movie.detail.{id}";
    }
}