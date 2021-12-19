namespace GMABot.Slash_Commands.Core
{
    public interface ISubcommand
    {
        DiscordCommandParameter[]? parameters { get; }

        void Execute(string token, string id, Dictionary<string,object>? parameters);
    }
}
