using Newtonsoft.Json;

namespace GMABot.Models.Reddit
{
    public class GalleryItem
    {
        [JsonProperty("media_id")]
        public string urlId { get; set; }
    }
}