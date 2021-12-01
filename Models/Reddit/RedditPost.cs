using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMABot.Models.Reddit
{
    internal class RedditPost
    {
        [JsonProperty("name")]
        public string id { get; set; }

        [JsonProperty("kind")]
        public string type { get; set; }

        [JsonProperty("url")]
        public string imageUrl { get; set; }

        [JsonProperty("permalink")]
        public string permaLink { get; set; }

        [JsonProperty("is_gallery")]
        public bool isGallery { get; set; }

        [JsonProperty("gallery_data")]
        public GalleryData? gallery { get; set; }

        [JsonProperty("is_video")]
        public bool isVideo { get; set; }
    }
}
