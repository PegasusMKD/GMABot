using Newtonsoft.Json;

namespace GMABot.Slash_Commands
{
    internal class DiscordSubcommand : DiscordCommandParameter
    {
        [JsonProperty(Order = 5)]
        public string? group { get; set; }

        [JsonProperty(Order = 6)]
        public DiscordSubcommand[]? options { get; set; }

        public DiscordSubcommand()
        {
            type = 1;
        }

        public DiscordSubcommand(DiscordCommandParameter parameter)
        {
            this.type = parameter.type;
            this.name = parameter.name;
            this.description = parameter.description;
            this.required = parameter.required;
        }
    }
}
