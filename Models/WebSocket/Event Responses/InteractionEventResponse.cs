using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMABot.Models.WebSocket.Event_Responses
{
    internal class InteractionEventResponse
    {
        public int type = 4;
        public DiscordMessage data { get; set; }
    }
}
