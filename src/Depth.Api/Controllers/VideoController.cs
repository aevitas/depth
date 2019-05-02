using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Depth.Client.YouTube.Abstractions;
using Depth.Client.YouTube.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Internal;

namespace Depth.Api.Controllers
{
    [Route("videos")]
    public class VideoController : Controller
    {
        private readonly IVideoSearchProvider _searchProvider;
        private readonly IMovieTrailerProvider _trailerProvider;

        public VideoController(IVideoSearchProvider videoSearchProvider, IMovieTrailerProvider movieTrailerProvider)
        {
            _searchProvider = videoSearchProvider ?? throw new ArgumentNullException(nameof(videoSearchProvider));
            _trailerProvider = movieTrailerProvider ?? throw new ArgumentNullException(nameof(movieTrailerProvider));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<IEnumerable<VideoEntry>>> Search(string query)
        {
            var result = await _searchProvider.SearchAsync(query);

            if (!result.Any())
                return NotFound();

            return Ok(result);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<VideoEntry>> Trailer(string movie)
        {
            var result = await _trailerProvider.GetTrailerAsync(movie);

            if (result == null)
                return NotFound();

            return Ok(result);
        }
    }
}
