using System.Collections.Generic;
using System.Threading.Tasks;
using Depth.Client.YouTube.Models;

namespace Depth.Client.YouTube.Abstractions
{
    public interface IVideoSearchProvider
    {
        Task<IEnumerable<VideoEntry>> SearchAsync(string query, int maximumResultCount = 10);
    }
}
