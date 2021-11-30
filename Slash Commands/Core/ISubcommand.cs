﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMABot.Slash_Commands.Core
{
    public interface ISubcommand
    {
        DiscordParameter[]? parameters { get; }

        void Execute(string channel, params object[] parameters);
    }
}