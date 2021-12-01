using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMABot.Models.WebSocket.Events
{
    internal class ReadyEvent
    {
        public string session_id { get; set; }
    }
}
