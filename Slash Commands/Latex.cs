using GMABot.Slash_Commands.Core;
namespace GMABot.Slash_Commands
{
    [Subcommand(Name = "latex", Description = "Get an image from r/ShinyPorn.")]
    internal class Latex : ISubcommand
    {
        public DiscordParameter[] parameters => new DiscordParameter[] { 
                new DiscordParameter { name = "category", description = "From which category should I choose: new, latest, top, rising" } 
            };

        public void Execute(string channel, params object[] parameters)
        {
            string category = parameters[0] as string ?? "new";
            // Get image from reddit
            // Send image to channel
        }
    }
}
