using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMABot.Models.Reddit
{
    internal class RedditPage
    {
        public RedditWrapper<RedditPost>[] children { get; set; }
    }
}
