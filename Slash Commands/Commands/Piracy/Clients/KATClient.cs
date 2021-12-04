using GMABot.Models.Torrents;

namespace GMABot.Slash_Commands.Commands.Piracy.Clients
{
    // https://kickass.onl/
    internal class KATClient : IPiracyClient
    {
        public async Task<Torrent[]> GetTopTorrents(string searchText, string? category, int size)
        {
            throw new NotImplementedException();
        }
    }
}
