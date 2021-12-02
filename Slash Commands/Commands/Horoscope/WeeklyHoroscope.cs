using GMABot.Factories;
using GMABot.HTTP;
using GMABot.Models.Schedules;
using GMABot.Slash_Commands.Core;

namespace GMABot.Slash_Commands.Commands.Horoscope
{
    [Subcommand(Name = "weekly", Group = "horoscope", Description = "Weekly horoscope info.")]
    internal class WeeklyHoroscope : ISubcommand
    {
        public DiscordCommandParameter[]? parameters => new DiscordCommandParameter[] {
                new DiscordCommandParameter { name = "horoscope-type", description = "Which horoscope are you interested (\"skorpija\" for example).", required = true }
            };

        // Add better formatting (with embed attributes)
        public void Execute(string token, string id, object[]? parameters)
        {
            Task.Factory.StartNew(() =>
            {
                string horoscopeType = (parameters![0] as String)!;
                Schedule schedule = new Schedule() { title = $"Weekly Horoscope - {horoscopeType}", type = FormatType.EMBED };
                var message = DiscordMessageFactory.CreateMessage(schedule, HTMLParser.GetDailyHoroscopeText($"http://www.astromagazin.com.mk/{horoscopeType}/nedelen.php", "nedelen"));
                DiscordHttpClient.ReplyToInteraction(token, id, message);
            });
        }
    }
}
