namespace GMABot.Slash_Commands
{
    public class DiscordParameter
    {
        public string? name { get; set; }
        public string? description { get; set; }
        public int? type { get; set; } = 3;
        public bool? required { get; set; } = false;
    }
}