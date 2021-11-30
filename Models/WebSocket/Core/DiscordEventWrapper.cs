namespace GMABot.Models.WebSocket
{
    class DiscordEventWrapper<T> : DiscordEventBase
    {
        public T d;

        public DiscordEventWrapper(int opCode, T data)
        {
            op = opCode;
            d = data;
        }
    }
}
