using System.Threading.Tasks;
using Depth.Client.MovieDb.Models;

namespace Depth.Client.MovieDb.Abstractions
{
    public interface IMovieDetailProvider
    {
        Task<MovieDetail> GetDetailAsync(int id);
    }
}
