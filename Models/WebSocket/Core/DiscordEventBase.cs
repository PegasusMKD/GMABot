﻿namespace GMABot.Models.WebSocket
{
    internal class DiscordEventBase
    {
        public DiscordEventType? t { get; set; } = null;
        public int op;
        public int? s;
        public string? json;
    }
}
