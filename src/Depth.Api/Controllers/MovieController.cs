using System;
using System.Threading.Tasks;
using Depth.Client.MovieDb.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace Depth.Api.Controllers
{
    [Route("movies")]
    public class MovieController : Controller
    {
        private readonly IMovieSearchProvider _searchProvider;

        public MovieController(IMovieSearchProvider searchProvider)
        {
            _searchProvider = searchProvider ?? throw new ArgumentNullException(nameof(searchProvider));
        }

        [Route("search")]
        public async Task<IActionResult> Search(string query, bool includeAdult = false)
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
    }
}
