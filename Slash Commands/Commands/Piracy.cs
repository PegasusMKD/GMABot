using GMABot.Slash_Commands.Core;

namespace GMABot.Slash_Commands.Commands
{
    [Subcommand(Name = "pirate", Description = "Search through multiple sites for a game, and then return the best url based on seeders & leechers.")]
    internal class Piracy : ISubcommand
    {
        public DiscordCommandParameter[]? parameters => new DiscordCommandParameter[] {
                new DiscordCommandParameter { name = "search-text", description = "The text you'd wish to be searched (for example, the name of the game).", required = true }
            };

        public void Execute(string token, string id, object[]? parameters)
        {
            throw new NotImplementedException();
        }
    }
}
