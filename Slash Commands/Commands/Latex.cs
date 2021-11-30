using GMABot.Slash_Commands.Core;

namespace GMABot.Slash_Commands.Commands
{
    [Subcommand(Name = "latex", Description = "Get an image from r/ShinyPorn.")]
    internal class Latex : ISubcommand
    {
        public DiscordCommandParameter[] parameters => new DiscordCommandParameter[] { 
                new DiscordCommandParameter { name = "category", description = "From which category should I choose: new, latest, top, rising" } 
            };

        public void Execute(string token, string id, object[] parameters)
        {
            string category = parameters[0] as string ?? "new";
            // Get image from reddit
            // Send image to channel
        }
    }
}
