using GMABot.Models.Torrents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMABot.Slash_Commands.Commands.Piracy.Clients
{
    internal interface IPiracyClient
    {
        Task<Torrent[]> GetTopTorrents(string searchText, string? category, int size);
    }
}
