using GMABot.Models;
using GMABot.Models.Discord;
using GMABot.Models.Schedules;
using GMABot.Models.Torrents;

namespace GMABot.Factories
{
    static internal class DiscordMessageFactory
    {

        public static DiscordMessage CreateMessage(Schedule schedule, string mainText)
        {
            var message = new DiscordMessage();

            if(schedule.type == FormatType.EMBED)
            {
                var embed = new DiscordEmbed()
                {
                    title = schedule.title,
                    type = schedule.embedType,
                    description = mainText
                };

                message.embeds = new List<DiscordEmbed> { embed };
            }
            else if(schedule.type == FormatType.MESSAGE)
            {
                message.content = mainText;
            }

            return message;
        }

        public static DiscordMessage CreateTopTorrentsMessage(List<Torrent> torrents) =>
            new() { embeds = torrents.Select(torrent => CreateEmbedMessage(torrent.name, torrent.GetDescription())).ToList() };

        public static DiscordEmbed CreateEmbedMessage(string title, string description) =>
            new() { title = title, description = description };

        public static DiscordMessage CreateMediaMessage(string alt, (string url, bool isVideo)[] urls)
        {
            var images = urls.Where(url => IsEmbeddable(url.url, url.isVideo)).Select(url => CreateMediaEmbed(url.url));
            var videos = string.Join("\n", urls.Where(url => !IsEmbeddable(url.url, url.isVideo)).Select(url => url.url));
            images = !images.Any() ? images : images.Concat(new List<DiscordEmbed>() { 
                new DiscordEmbed { description = $"In case of failure: https://www.reddit.com{alt}" } 
            }).ToList();

            return new()
            {
                content = videos,
                embeds = images.ToList()
            };
        }

        public static bool IsEmbeddable(string url, bool isVideo) =>
            !url.Contains("redgifs") && !url.EndsWith("gifv") && 
            !url.Contains("youtube") && !url.StartsWith("https://imgur.com/");

        public static DiscordEmbed CreateMediaEmbed(string url) =>
            new() { type = EmbedType.IMAGE, image = new DiscordImage { url = url } };
    }
}
