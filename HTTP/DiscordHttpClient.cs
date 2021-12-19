using GMABot.Factories;
using GMABot.Models;
using GMABot.Models.WebSocket.Event_Responses;
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
        static readonly HttpClient client = HttpClientFactory.GetDiscordHttpClient();

        // Most probably not needed here!
        private const string applicationId = "913933589392023584";
        // 

        private static readonly JsonSerializerSettings serializerSettings = new()
        {
            NullValueHandling = NullValueHandling.Ignore
        };

        public static void ReplyToInteraction(string interactionToken, string interactionId, DiscordMessage reply)
        {
            Console.WriteLine($"[{DateTime.Now}] Replied to interaction: {interactionId}");
            var request = new HttpRequestMessage(HttpMethod.Post,
                HttpClientFactory.baseUri + $"/v8/interactions/{interactionId}/{interactionToken}/callback");

            request.Content = new StringContent(JsonConvert.SerializeObject(new InteractionEventResponse { data = reply }), Encoding.Unicode, "application/json");

            client.SendAsync(request);
        }

        public static void CreateCommand(DiscordSubcommand subcommand)
        {
            var request = new HttpRequestMessage(HttpMethod.Post,
                HttpClientFactory.baseUri + $"/applications/{applicationId}/commands");
            var json = JsonConvert.SerializeObject(subcommand, serializerSettings);
            request.Content = new StringContent(json, Encoding.Unicode, "application/json");
            var result = client.SendAsync(request).Result;
            if (!result.IsSuccessStatusCode)
                Console.Error.WriteLine("Something went terribly wrong...");
        }

        public static void SendTimerMessage(MessageTimer? timer, DiscordMessage message, string channel)
        {
            if (timer == null) return;

            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
            if (!currentTime.IsBetween(timer.Time, timer.Time.AddMinutes(2))) return;

            SendMessage(message, channel);

            Console.WriteLine($"[{DateTime.Now}] Sent message.");
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
