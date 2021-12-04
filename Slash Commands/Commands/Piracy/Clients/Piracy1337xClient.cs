using GMABot.Factories;
using GMABot.Models.Torrents;
using HtmlAgilityPack;

namespace GMABot.Slash_Commands.Commands.Piracy.Clients
{
    // https://1337x.to/
    public class Piracy1337xClient : IPiracyClient
    {
        public readonly HttpClient client = HttpClientFactory.GetDefaultUserAgentHttpClient();
        public string[] specialCategories = { "xxx", "tv" };

        public const string baseUri = "https://1337x.to";

        public async Task<Torrent[]> GetTopTorrents(string searchText, string? category, int size)
        {
            string queryText = string.Join("+",searchText.Split(" "));
            if (category != null && specialCategories.Contains(category.ToLower()))
                category = category.ToUpper();
            else if(category != null)
                category = string.Concat(category[0].ToString().ToUpper(), category.AsSpan(1));

            List<Torrent> torrents = new();
            int page = 1;
            while(torrents.Count < size)
            {
                var torrentPage = await GetTorrentsPage(queryText, category, page++);
                if (torrentPage == null) break;
                torrents.AddRange(torrentPage);
            }
           
            return torrents.ToArray();
        }

        public async Task<Torrent[]> GetTorrentsPage(string searchText, string category, int page)
        {
            var html = await GetTorrentsPageHtml(searchText, category, page);
            HtmlDocument document = new();
            document.LoadHtml(html);
            HtmlNode documentNode = document.DocumentNode;
            
            var torrents = documentNode.SelectNodes("//a[starts-with(@href, '/torrent/')]");
            var users = documentNode.SelectNodes("//a[starts-with(@href, '/user/')]");
            var seeders = documentNode.SelectNodes("//td[contains(concat(' ', normalize-space(@class), ' '), ' seeds ')]");
            var leechers = documentNode.SelectNodes("//td[contains(concat(' ', normalize-space(@class), ' '), ' leeches ')]");

            return torrents.Zip(users, (torrent, user) => 
                new Torrent { name = torrent.InnerText, uploader = user.InnerText,
                    url = baseUri + torrent.Attributes["href"].Value }
                ).Zip(seeders, (torrent, seeder) => { torrent.seeders = int.Parse(seeder.InnerText); return torrent; })
                .Zip(leechers, (torrent, leecher) => { torrent.leechers = int.Parse(leecher.InnerText); return torrent; })
                .ToArray();
        }

        public async Task<string> GetTorrentsPageHtml(string searchText, string? category, int page)
        {
            var url = $"{baseUri}/sort{(category != null ? "-category" : "")}-search/{searchText}{(category != null ? "/" + category : "")}/seeders/desc/{page}/";
            return await client.GetStringAsync(url);
        }

    }
}
