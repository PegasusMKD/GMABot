using Newtonsoft.Json;

namespace GMABot.Slash_Commands
{
    public class DiscordCommandParameter
    {
        [JsonProperty(Order = 1)]
        public string? name { get; set; }

        [JsonProperty(Order = 2)]
        public string? description { get; set; }

        [JsonProperty(Order = 3)]
        public int? type { get; set; } = 3;

        [JsonProperty(Order = 4)]
        public bool? required { get; set; } = false;
    }
}