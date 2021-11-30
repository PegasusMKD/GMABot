using GMABot.Factories;
using GMABot.Models;
using GMABot.Slash_Commands;
using GMABot.Timers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMABot.HTTP
{
    static internal class DiscordHttpClient
    {
        static readonly HttpClient client = HttpClientFactory.GetHttpClient();

        // Most probably not needed here!
        private const string applicationId = "913933589392023584";
        // 

        private static readonly JsonSerializerSettings serializerSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public static void CreateCommand(DiscordSubcommand subcommand)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                HttpClientFactory.baseUri + $"/applications/{applicationId}/commands");
            request.Content = new StringContent(JsonConvert.SerializeObject(subcommand, serializerSettings), Encoding.Unicode, "application/json");

            client.SendAsync(request);
        }

        public static void SendTimerMessage(MessageTimer? timer, DiscordMessage message, string channel)
        {
            if (timer == null) return;

            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
            if (!currentTime.IsBetween(timer.Time, timer.Time.AddMinutes(2))) return;

            SendMessage(message, channel);

            timer.Stop();
        }

        public static void SendMessage(DiscordMessage message, string channel)
        {
            Console.WriteLine($"[{DateTime.Now}] Sent message: {message}");

            var request = new HttpRequestMessage(HttpMethod.Post,
                HttpClientFactory.baseUri + $"/channels/{channel}/messages");

            request.Content = new StringContent(JsonConvert.SerializeObject(message), Encoding.Unicode, "application/json");

            client.SendAsync(request);

        }
    }
}
