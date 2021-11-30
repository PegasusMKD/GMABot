using GMABot.Factories;
using GMABot.HTTP;
using GMABot.Models.Schedules;
using GMABot.Slash_Commands.Core;

namespace GMABot.Slash_Commands
{
    [Subcommand(Name = "daily", Group = "horoscope", Description = "Daily horoscope info.")]
    internal class DailyHoroscope : ISubcommand
    {
        public DiscordParameter[]? parameters => new DiscordParameter[] {
                new DiscordParameter { name = "horoscope-type", description = "Which horoscope are you interested (\"skorpija\" for example).", required = true }
            };

        public void Execute(string channel, params object[] parameters)
        {
            Task.Factory.StartNew(() =>
            {
                string horoscopeType = (parameters[0] as String)!;
                Schedule schedule = new Schedule() { title = $"Horoscope - {horoscopeType}", type = FormatType.EMBED };
                var message = DiscordMessageFactory.CreateMessage(schedule, HTMLParser.ParseHtmlText($"http://www.astromagazin.com.mk/{horoscopeType}/dneven.php"));
                DiscordHttpClient.SendMessage(message, channel);
            });
        }
    }
}
