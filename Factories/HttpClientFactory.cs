using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace GMABot.Factories
{
    // Client Factory so we don't have to always copy-paste the token, or try to reference it in the WS code, from the HTTP codebase
    static internal class HttpClientFactory
    {
        private const string discordToken = "OTEzOTMzNTg5MzkyMDIzNTg0.YaFs-w.87wQwPP2ISCHroQqU-93kXvFCBI";
        public const string baseUri = "https://discord.com/api";

        public static HttpClient GetDiscordHttpClient() => GetHttpClient("Bot", discordToken, null);
        public static HttpClient GetBearerHttpClient(string token) => GetHttpClient("Bearer", token, null);
        public static HttpClient GetRedditHttpClient(string token) => GetHttpClient("Bearer", token, "GMABot/1.0.0");

        public static HttpClient GetHttpClient(string tokenType, string token, string? userAgent)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);
            if(userAgent != null) client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);
            return client;
        }
    }
}
