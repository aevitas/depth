using System.Threading.Tasks;
using Depth.Client.YouTube.Models;

namespace Depth.Client.YouTube.Abstractions
{
    public interface IMovieTrailerProvider
    {
        Task<VideoEntry> GetTrailerAsync(string movie);
    }
}
