namespace GMABot.Models.WebSocket.Events
{
    internal class InteractionEvent
    {
        public string token;
        public string id;
        public DiscordEventParameter data;
    }
}
