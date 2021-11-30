using Newtonsoft.Json;

namespace GMABot.Models
{
    struct DiscordProperties
    {
        [JsonProperty("$os")]
        public string os;

        [JsonProperty("$browser")]
        public string browser;

        [JsonProperty("$device")]
        public string device;
    }
}
