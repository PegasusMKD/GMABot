using GMABot.Factories;
using GMABot.Models.Reddit;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMABot.Slash_Commands.Commands.Latex
{
    // https://www.reddit.com/dev/api
    // https://www.reddit.com/prefs/apps
    static class RedditClient
    {

        private const string baseUri = "https://oauth.reddit.com";

        private const string clientId = "xDHTW0tDFPMnhROm02qtCQ";
        private const string secret = "yG_7RLw_J4NTEKtaeM9-gx8CQXCw2g";
        private static readonly string clientSecret = Convert.ToBase64String(Encoding.ASCII.GetBytes($"{clientId}:{secret}"));

        private static string token;
        private static System.Timers.Timer resetTokenTimer;

        private static readonly Random random = new();
        private static string? latestPostId;
        private static HashSet<string> selectedPosts = new();

        private static async Task Login()
        {
            HttpClient client = new();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://www.reddit.com/api/v1/access_token");
            request.Content = new FormUrlEncodedContent(new Dictionary<string, string> { { "grant_type", "client_credentials" } });
            request.Headers.Add("Authorization", $"Basic {clientSecret}");
            var result = await client.SendAsync(request);
            if (!result.IsSuccessStatusCode) throw new Exception("Reddit client couldn't log in!");

            var oauth = JsonConvert.DeserializeObject<RedditOauth>(await result.Content.ReadAsStringAsync());
            token = oauth!.access_token;
            if (resetTokenTimer != null && resetTokenTimer.Enabled) return;

            resetTokenTimer = new System.Timers.Timer(oauth.expires_in * 1000);
            resetTokenTimer.Elapsed += new System.Timers.ElapsedEventHandler(async (e, v) => await Login());
            resetTokenTimer.Start();
        }


        public static async Task<RedditPost> GetRandomPost(string subreddit, string category, bool forceUrl = false)
        {
            if(token == null) await Login();
            var wrapper = await GetRandomNumberOfPosts(subreddit, category);
            var page = wrapper.data;
            var actualChildren = page.children.Select(post => post.data).ToArray();

            var selected = actualChildren[random.Next(0, page.children.Length - 1)];
            bool internalReset = false;
            while (selectedPosts.Contains(selected.id) || internalReset)
            {
                internalReset = false;
                selected = actualChildren[random.Next(0, page.children.Length - 1)];
                if(forceUrl && (selected.imageUrl == null || selected.imageUrl == "")) 
                    internalReset = true;
            }

            latestPostId = selected.id;
            selectedPosts.Add(selected.id);
            return selected;
        }

        private static async Task<RedditWrapper<RedditPage>> GetRandomNumberOfPosts(string subreddit, string category)
        {
            int nextType = random.Next();
            nextType = (nextType << 3) + random.Next();
            string type = nextType % 2 == 0 ? "after" : "before";

            int limit = random.Next(1, 100);
            string skip = latestPostId == null ? string.Empty : $"{type}={latestPostId}&";

            Console.WriteLine($"Reddit URL: {baseUri}/{subreddit}/{category}?{skip}limit={limit}");

            HttpClient client = HttpClientFactory.GetRedditHttpClient(token);
            var request = new HttpRequestMessage(HttpMethod.Get, $"{baseUri}/{subreddit}/{category}?{skip}limit={limit}");
            var response = await client.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(await response.Content.ReadAsStringAsync());
                throw new Exception("Reddit Client couldn't get pages!");
            }

            return JsonConvert.DeserializeObject<RedditWrapper<RedditPage>>(await response.Content.ReadAsStringAsync())!;
        }
    }
}
