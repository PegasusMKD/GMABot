namespace GMABot.Slash_Commands
{
    internal class DiscordSubcommand : DiscordParameter
    {
        public string? group { get; set; }
        
        public List<DiscordSubcommand>? options { get; set; }

        public DiscordSubcommand()
        {
            type = 1;
        }

        public DiscordSubcommand(DiscordParameter parameter)
        {
            this.type = parameter.type;
            this.name = parameter.name;
            this.description = parameter.description;
            this.required = parameter.required;
        }
    }
}
