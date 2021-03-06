﻿using System;

namespace Depth.Client.YouTube.Models
{
    public class VideoEntry
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public DateTimeOffset? PublishedAt { get; set; }
    }
}
