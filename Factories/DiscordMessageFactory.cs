using GMABot.Models;
using GMABot.Models.Discord;
using GMABot.Models.Schedules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public static DiscordMessage CreateMediaMessage(string alt, (string url, bool isVideo)[] urls) =>
            new() { embeds = urls.Select(url => CreateMediaEmbed(url.url, url.isVideo)).Concat(new List<DiscordEmbed>() {
                new DiscordEmbed { description = $"In case of failure: https://www.reddit.com{alt}" }
            }).ToList() };

        public static DiscordEmbed CreateMediaEmbed(string url, bool isVideo) =>
            new() { type = EmbedType.IMAGE, image = new DiscordImage { url = url } };
    }
}
