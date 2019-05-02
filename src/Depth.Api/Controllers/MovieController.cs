using System;
using System.Threading.Tasks;
using Depth.Client.MovieDb.Abstractions;
using Depth.Client.MovieDb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace Depth.Api.Controllers
{
    [Route("movies")]
    public class MovieController : Controller
    {
        private readonly IMovieSearchProvider _searchProvider;
        private readonly IMovieDetailProvider _detailProvider;

        public MovieController(IMovieSearchProvider searchProvider, IMovieDetailProvider detailProvider)
        {
            _searchProvider = searchProvider ?? throw new ArgumentNullException(nameof(searchProvider));
            _detailProvider = detailProvider ?? throw new ArgumentNullException(nameof(detailProvider));
        }

        [HttpGet("search")]
        public async Task<ActionResult<MovieEntry>> Search(string query, bool includeAdult = false)
        {
            var result = await _searchProvider.SearchAsync(opts =>
            {
                opts.Query = query;
                opts.IncludeAdult = includeAdult;
            });

            if (result == null || !result.Any())
                return NotFound();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MovieDetail>> Detail(int id)
        {
            var result = await _detailProvider.GetDetailAsync(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
