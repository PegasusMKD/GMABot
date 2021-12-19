using GMABot.Factories;
using GMABot.Models.Torrents;
using HtmlAgilityPack;
using System.Linq;

namespace GMABot.Slash_Commands.Commands.Piracy.Clients
{
    // https://kickasstorrents.to
    internal class KATClient : IPiracyClient
    {

        public readonly HttpClient client = HttpClientFactory.GetDefaultUserAgentHttpClient();

        public const string baseUri = "https://kickasstorrents.to";

        public async Task<Torrent[]> GetTopTorrents(string searchText, string? category, int size)
        {
            if (category != null)
                category = category.ToLower();

            List<Torrent> torrents = new();
            int page = 1;
            while (torrents.Count < size)
            {
                Torrent[] torrentPage;
                try
                {
                    torrentPage = await GetTorrentsPage(searchText, category, page++);
                }
                catch (Exception)
                {
                    return torrents.ToArray();
                }

                if (torrentPage == null) break;
                torrents.AddRange(torrentPage);
            }

            return torrents.ToArray();
        }

        private async Task<Torrent[]> GetTorrentsPage(string searchText, string? category, int page)
        {
            var html = await GetTorrentsPageHtml(searchText, category, page);
            if(html == null) 
                throw new Exception();

            HtmlDocument document = new();
            document.LoadHtml(html);
            HtmlNode documentNode = document.DocumentNode;

            var torrentTable = documentNode.SelectSingleNode("//table[contains(concat(' ', normalize-space(@class), ' '), ' data ')]");
            var torrentRows = torrentTable.ChildNodes[1].ChildNodes.Where(row => row.Name == "tr").Skip(1);

            return (from row in torrentRows
                    let torrentNameNode = row.SelectSingleNode("//a[contains(concat(' ', normalize-space(@class), ' '), ' cellMainLink ')]")
                    let name = torrentNameNode.InnerText
                    let url = baseUri + torrentNameNode.Attributes["href"].Value
                    let td = row.ChildNodes.Where(_row => _row.Name == "td").ToArray()
                    let uploader = td[2].InnerText
                    let seeders = int.Parse(td[4].InnerText)
                    let leechers = int.Parse(td[5].InnerText)
                    select new Torrent { name = name, url = url, uploader = uploader, seeders = seeders, leechers = leechers }).ToArray();
        }

        private async Task<string?> GetTorrentsPageHtml(string searchText, string? category, object page)
        {
            var url = $"{baseUri}/search/{searchText}{(category != null ? "/category" : "")}{(category != null ? "/" + category : "")}/{page}?sortby=seeders&sort=desc";
            var request = await client.GetAsync(url);
            if (request != null && !request.IsSuccessStatusCode)
                return null;

            return await request?.Content.ReadAsStringAsync()!;
        }
        
    }
}
