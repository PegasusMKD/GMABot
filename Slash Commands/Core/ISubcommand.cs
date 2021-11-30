using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMABot.Slash_Commands.Core
{
    public interface ISubcommand
    {
        DiscordCommandParameter[]? parameters { get; }

        void Execute(string token, string id, object[] parameters);
    }
}
