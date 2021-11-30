using GMABot.Factories;
using GMABot.HTTP;
using GMABot.Models.Schedules;
using GMABot.Slash_Commands.Core;

namespace GMABot.Slash_Commands
{
    [Subcommand(Name = "weekly", Group = "horoscope", Description = "Weekly horoscope info.")]
    internal class WeeklyHoroscope : ISubcommand
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
                var message = DiscordMessageFactory.CreateMessage(schedule, HTMLParser.ParseHtmlText($"http://www.astromagazin.com.mk/{horoscopeType}/nedelen.php"));
                DiscordHttpClient.SendMessage(message, channel);
            });
        }
    }
}
