using GMABot.Factories;
using GMABot.Models.Torrents;
using HtmlAgilityPack;

namespace GMABot.Slash_Commands.Commands.Piracy.Clients
{
    // https://thepiratebay0.org/search/DOOM/1/99/300,400
    // URL/search-text/<page>/7/<category-id-list using ',' as a separator>
    internal class PirateBayClient : IPiracyClient
    {
        public readonly HttpClient client = HttpClientFactory.GetDefaultUserAgentHttpClient();

        public const string baseUri = "https://thepiratebay0.org";

        public async Task<Torrent[]> GetTopTorrents(string searchText, string? category, int size)
        {
            PirateBayCategory cat = PirateBayCategory.DEFAULT;
            if (category != null)
                cat = PirateBayCategoryConverter.GetType(category.ToLower());

            return await FillTorrentsList(searchText, size, cat);
        }

        public async Task<Torrent[]> FillTorrentsList(string searchText, int size, PirateBayCategory category)
        {
            List<Torrent> torrents = new();
            int page = 1;
            while (torrents.Count < size)
            {
                Torrent[] torrentPage;
                try
                {
                    torrentPage = await GetTorrentsPage(searchText, category, page++);
                } catch (Exception)
                {
                    return torrents.ToArray();
                }
                if (torrentPage == null) break;
                torrents.AddRange(torrentPage);
            }

            return torrents.ToArray();
        }

        public async Task<Torrent[]> GetTorrentsPage(string searchText, PirateBayCategory category, int page)
        {
            var html = await GetTorrentsPageHtml(searchText, category, page);
            HtmlDocument document = new();
            document.LoadHtml(html);
            HtmlNode documentNode = document.DocumentNode;

            var torrents = documentNode.SelectNodes("//a[contains(concat(' ', normalize-space(@class), ' '), ' detLink ')]");
            if (torrents == null || torrents.Count == 0)
                throw new Exception();
            var users = documentNode.SelectNodes($"//a[starts-with(@href, '{baseUri}/user/')]");

            var seedersAndLeechers = documentNode.SelectNodes("//td[contains(concat(' ', normalize-space(@align), ' '), ' right ')]");
            var seeders = seedersAndLeechers.Where((val, idx) => idx % 2 == 0);
            var leechers = seedersAndLeechers.Where((val, idx) => idx % 2 == 1);

            return torrents.Zip(users, (torrent, user) =>
                new Torrent
                {
                    name = torrent.InnerText,
                    uploader = user.InnerText,
                    url = torrent.Attributes["href"].Value
                }
                ).Zip(seeders, (torrent, seeder) => { torrent.seeders = int.Parse(seeder.InnerText); return torrent; })
                .Zip(leechers, (torrent, leecher) => { torrent.leechers = int.Parse(leecher.InnerText); return torrent; })
                .ToArray();
        }

        public async Task<string> GetTorrentsPageHtml(string searchText, PirateBayCategory category, int page)
        {
            var url = $"{baseUri}/search/{searchText}/{page}/7/{(int?)category}";
            return await client.GetStringAsync(url);
        }
    }
}
