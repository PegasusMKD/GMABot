namespace GMABot.Models.Torrents
{
    public class Torrent
    {
        public string name { get; set; }
        public string uploader { get; set; }
        public string url { get; set; }
        public int seeders { get; set; }
        public int leechers { get; set; }

        internal string GetDescription()
        {
            return 
                $"\t **Uploader: {uploader}**\n\t " +
                $"URL: {url}\n\t " +
                $"Seeders: {seeders}\n\t " +
                $"Leechers: {leechers} \n\t";
        }
    }
}
