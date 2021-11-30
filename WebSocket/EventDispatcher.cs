using GMABot.Models.WebSocket;
using GMABot.Models.WebSocket.Events;
using GMABot.Slash_Commands.Core;
using Newtonsoft.Json;

namespace GMABot.WebSocket
{
    static class EventDispatcher
    {
        public static Dictionary<string, ISubcommand> commands = new();

        public static void Dispatch(DiscordEventBase eventBase)
        {
            if (eventBase.op == 11) eventBase.t = DiscordEventType.HEARTBEAT;
            if (eventBase == null) return;
            Console.WriteLine($"[{DateTime.Now}] Received event: {eventBase.t}");
            switch(eventBase.t)
            {
                case DiscordEventType.INTERACTION_CREATE:
                    var discordEvent = GetCommand<InteractionEvent>(eventBase, (discordEvent) => discordEvent.data);
                    commands[discordEvent.command.name].Execute(discordEvent.meta.token, discordEvent.meta.id,
                        discordEvent.command.options!.Select(option => option.value).ToArray());
                    break;

                // Disconnected, should close all sockets, reconnect and resume
                case DiscordEventType.READY:
                    var readyEvent = JsonConvert.DeserializeObject<DiscordEventWrapper<ReadyEvent>>(eventBase.json!);
                    DiscordWebSocket.sessionId = readyEvent!.d!.session_id;
                    break;
            }
        }

        public static (T meta, DiscordEventParameter command) GetCommand<T>(DiscordEventBase eventBase, Func<T, DiscordEventParameter> startingParameters)
        {
            var discordEvent = JsonConvert.DeserializeObject<DiscordEventWrapper<T>>(eventBase.json!);
            DiscordEventParameter command = startingParameters.Invoke(discordEvent!.d!);
            DiscordEventParameter? previous = null;
            while (command.options != null)
            {
                previous = command;
                command = command.options[0];
            }
            return (discordEvent.d, previous ?? command)!;
        }
    }
}
