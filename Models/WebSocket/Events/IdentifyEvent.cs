namespace GMABot.Models.WebSocket.Events
{
    struct IdentifyEvent
    {
        public string token;
        public int intents;
        public DiscordProperties properties;
    }
}
