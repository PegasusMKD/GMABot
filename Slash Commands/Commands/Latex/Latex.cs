﻿using GMABot.Factories;
using GMABot.HTTP;
using GMABot.Models.Reddit;
using GMABot.Models.Schedules;
using GMABot.Slash_Commands.Core;

namespace GMABot.Slash_Commands.Commands.Latex
{
    [Subcommand(Name = "latex", Description = "Get an image from r/ShinyPorn.")]
    internal class Latex : ISubcommand
    {
        public DiscordCommandParameter[] parameters => new DiscordCommandParameter[] { 
                new DiscordCommandParameter { name = "category", description = "From which category should I choose: new, latest, top, rising" } 
            };

        // Reddit Login
        // Get "random" post from Reddit
        // Get Binary image of post
        // Send image as Discord Message
        // Path: https://www.reddit.com/r/ShinyPorn/{category}?count=0
        public void Execute(string token, string id, Dictionary<string, object>? parameters)
        {
            Task.Factory.StartNew(async () =>
            {
                string category = parameters.ContainsKey("category") ? (parameters?["category"] as string)! : "new";
                RedditPost post = await RedditClient.GetRandomPost("r/ShinyPorn", category, true);
                Console.WriteLine($"[{DateTime.Now}] Post PermaUrl: {post.permaLink}");
                Console.WriteLine($"[{DateTime.Now}] Image Url: {post.imageUrl}");

                var urls = new (string, bool)[] { (post.imageUrl, post.isVideo) };
                if(post.isGallery)
                    urls = post.gallery!.items!.Select(item => ($"https://i.redd.it/{item.urlId}.jpg", false)).ToArray();

                DiscordHttpClient.ReplyToInteraction(token, id, DiscordMessageFactory.CreateMediaMessage(post.permaLink, urls));
            });
        }
    }
}
