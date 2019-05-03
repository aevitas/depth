using System;

namespace Depth.Api.Models
{
    public class TrailerModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTimeOffset? PublishedAt { get; set; }

        public string Uri { get; set; }
    }
}
