using GMABot.Factories;
using GMABot.Models;
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

        public static void SendMessage(MessageTimer? timer, DiscordMessage message, string channel)
        {
            if (timer == null) return;

            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
            if (!currentTime.IsBetween(timer.Time, timer.Time.AddMinutes(2))) return;

            Console.WriteLine($"[{DateTime.Now}] Sent message: {message}");

            var request = new HttpRequestMessage(HttpMethod.Post,
                HttpClientFactory.baseUri + $"/channels/{channel}/messages");

            request.Content = new StringContent(JsonConvert.SerializeObject(message), Encoding.Unicode, "application/json");

            client.Send(request);
            timer.Stop();
        }
    }
}
