namespace GMABot.Models.WebSocket
{
    internal class DiscordEventParameter
    {
        public int type { get; set; }
        public string name { get; set; }
        public object value { get; set; }
        public DiscordEventParameter[]? options { get; set; }
    }
}
