using GMABot.Models.Torrents;

namespace GMABot.Slash_Commands.Commands.Piracy.Clients
{
    // https://thepiratebay0.org/search/DOOM/1/99/300,400
    // URL/search-text/1/7/<category-id-list using ',' as a separator>
    internal class PirateBayClient : IPiracyClient
    {
        public async Task<Torrent[]> GetTopTorrents(string searchText, string? category, int size)
        {
            throw new NotImplementedException();
        }
    }
}
