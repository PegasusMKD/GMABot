using System.Net.Http.Headers;

namespace GMABot.Factories
{
    // Client Factory so we don't have to always copy-paste the token, or try to reference it in the WS code, from the HTTP codebase
    static internal class HttpClientFactory
    {
        private const string discordToken = "OTEzOTMzNTg5MzkyMDIzNTg0.YaFs-w.87wQwPP2ISCHroQqU-93kXvFCBI";
        public const string baseUri = "https://discord.com/api";
        public const string defaultUserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/96.0.4664.45 Safari/537.36";

        public static HttpClient GetDiscordHttpClient() => GetHttpClient("Bot", discordToken, null);
        public static HttpClient GetBearerHttpClient(string token) => GetHttpClient("Bearer", token, null);
        public static HttpClient GetRedditHttpClient(string token) => GetHttpClient("Bearer", token, "GMABot/1.0.0");
        public static HttpClient GetUserAgentHttpClient(string userAgent) => GetHttpClient(null, null, userAgent);
        public static HttpClient GetDefaultUserAgentHttpClient() => GetHttpClient(null, null, defaultUserAgent);

        public static HttpClient GetHttpClient(string? tokenType, string? token, string? userAgent)
        {
            HttpClient client = new();

            if(tokenType != null && token != null) 
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(tokenType, token);

            if(userAgent != null) 
                client.DefaultRequestHeaders.UserAgent.ParseAdd(userAgent);

            return client;
        }
    }
}
