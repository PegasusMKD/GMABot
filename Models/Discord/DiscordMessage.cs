namespace GMABot.Models
{
    // Discord Message Structure (only relevant properties)
    // https://discord.com/developers/docs/resources/channel#create-message
    struct DiscordMessage
    {
        public string content = "";
        public bool tts = false;
        public bool allow_mentions = false;
        public List<DiscordEmbed>? embeds = null;
    }
}