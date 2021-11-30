using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMABot.Models.WebSocket.Events
{
    internal class InteractionEvent
    {
        public string token;
        public string id;
        public DiscordEventParameter data;
    }
}
