using GMABot.Factories;
using GMABot.HTTP;
using GMABot.Models.Torrents;
using GMABot.Slash_Commands.Commands.Piracy.Clients;
using GMABot.Slash_Commands.Core;

namespace GMABot.Slash_Commands.Commands.Piracy
{
    [Subcommand(Name = "pirate", Description = "Search through multiple sites for a game, and then return the best url based on seeders & leechers.")]
    internal class Piracy : ISubcommand
    {
        private const int siteSize = 25;

        public DiscordCommandParameter[]? parameters => new DiscordCommandParameter[] {
                new DiscordCommandParameter { name = "search-text", description = "The text you'd wish to be searched (for example, the name of the game).", required = true },
                new DiscordCommandParameter { name = "category", description = "The category to use (games, movies, etc.)." },
                new DiscordCommandParameter { name = "algorithm", description = "The algorithm to use when comparing torrents (ratio, seeders, leechers, formula)." }
            };

        private readonly IPiracyClient[] clients = new IPiracyClient[] { 
            new Piracy1337xClient(),
            new KATClient(),
            new PirateBayClient()
        };

        public void Execute(string token, string id, Dictionary<string, object>? parameters)
        {
            Task.Factory.StartNew(async () => {
                var searchText = (parameters!["search-text"] as string)!;
                string? category = parameters.ContainsKey("category") ? parameters!["category"] as string : null;
                PiracyAlgorithm algorithm = parameters.ContainsKey("algorithm") ?
                    PiracyAlgorithmConverter.GetType((parameters!["algorithm"] as string)!.ToLower()) :
                    PiracyAlgorithm.FORMULA;

                var torrents = new List<Torrent>();
                foreach(var client in clients)
                    torrents.AddRange(await client.GetTopTorrents(searchText, category, siteSize));

                IEnumerable<Torrent> torrentQuery = torrents.AsQueryable();
                switch(algorithm)
                {
                    case PiracyAlgorithm.RATIO:
                        torrentQuery = torrents.OrderByDescending(torrent => torrent.seeders / (float)(torrent.seeders + torrent.leechers));
                        break;
                    case PiracyAlgorithm.SEEDERS:
                        torrentQuery = torrents.OrderByDescending(torrent => torrent.seeders);
                        break;
                    case PiracyAlgorithm.LEECHERS:
                        torrentQuery = torrents.OrderByDescending(torrent => torrent.leechers);
                        break;
                    case PiracyAlgorithm.FORMULA:
                        torrentQuery = torrents.OrderByDescending(torrent => torrent.seeders * 6 - torrent.leechers * 2);
                        break;
                }

                torrents = torrentQuery.Take(5).ToList();
                DiscordHttpClient.ReplyToInteraction(token, id, DiscordMessageFactory.CreateTopTorrentsMessage(torrents));
            });
        }
    }
}
