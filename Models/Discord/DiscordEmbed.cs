using GMABot.Models.Discord;

namespace GMABot.Models
{
    public class DiscordEmbed
    {
        public string? title;
        public EmbedType type;
        public string? description;

        public string? url;
        public DiscordImage? image;
        public DiscordImage? video;
    }
}