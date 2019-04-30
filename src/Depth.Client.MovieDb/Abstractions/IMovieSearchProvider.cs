using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Depth.Client.MovieDb.Models;

namespace Depth.Client.MovieDb.Abstractions
{
    public interface IMovieSearchProvider
    {
        Task<IEnumerable<Movie>> SearchAsync(Action<MovieQueryOptions> options);
    }
}
