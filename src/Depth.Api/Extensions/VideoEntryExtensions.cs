using Depth.Api.Models;
using Depth.Client.YouTube.Models;

namespace Depth.Api.Extensions
{
    internal static class VideoEntryExtensions
    {
        public static TrailerModel ToTrailerModel(this VideoEntry entry)
        {
            return new TrailerModel
            {
                PublishedAt = entry.PublishedAt,
                Description = entry.Description,
                Title = entry.Title,
                Uri = $"https://www.youtube.com/watch?v={entry.Id}"
            };
        }
    }
}
