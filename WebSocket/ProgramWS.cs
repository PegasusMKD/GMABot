using Newtonsoft.Json;
using System.Net.WebSockets;
using System.Text;

class WS
{
    async Task GetDiscordData(ClientWebSocket clientWebSocket)
    {
        CancellationToken cancellationToken = new CancellationToken();

        var buffer = new byte[1024];
        ArraySegment<byte> bufferWrapper = new ArraySegment<byte>(buffer);

        var dataBuffer = new byte[4096];
        MemoryStream data = new(dataBuffer);

        while (true)
        {
            var received = await clientWebSocket.ReceiveAsync(buffer, cancellationToken);
            if (received.MessageType != WebSocketMessageType.Close)
                data.Write(bufferWrapper.Array, 0, received.Count);

            if (received.MessageType == WebSocketMessageType.Close || received.EndOfMessage)
                break;
        }

        data.Position = 0;
        var jsonString = Encoding.UTF8.GetString(data.ToArray());
        var json = JsonConvert.DeserializeObject(jsonString);

        Console.WriteLine(json);

        data.Dispose();
        data.Close();
    }

    async Task CloseSockets(ClientWebSocket clientWebSocket)
    {
        await clientWebSocket.CloseOutputAsync(WebSocketCloseStatus.Empty, "", CancellationToken.None);
        await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
    }


    async Task SendDiscordData<T>(ClientWebSocket clientWebSocket, DiscordPayload<T> payload)
    {
        CancellationToken cancellationToken = new();

        var jsonPayload = JsonConvert.SerializeObject(payload);

        var payloadBuffer = Encoding.UTF8.GetBytes(jsonPayload);
        var payloadBufferWrapper = new ArraySegment<byte>(payloadBuffer);

        await clientWebSocket.SendAsync(payloadBufferWrapper, WebSocketMessageType.Text, true, cancellationToken);
    }

    async Task Main()
    {
        CancellationToken cancellationToken = new();
        ClientWebSocket clientWebSocket = new ClientWebSocket();
        await clientWebSocket.ConnectAsync(new Uri("wss://gateway.discord.gg/?v=9&encoding=json"), cancellationToken);

        GetDiscordData(clientWebSocket).Wait();

        var payload = new DiscordPayload<int>(1, 2);
        SendDiscordData<int>(clientWebSocket, payload).Wait();
        GetDiscordData(clientWebSocket).Wait();


        var properties = new DiscordProperties()
        {
            os = "linux",
            browser = "my_library",
            device = "my_library"
        };

        var identify = new Identify()
        {
            token = "OTEzOTMzNTg5MzkyMDIzNTg0.YaFs-w.87wQwPP2ISCHroQqU-93kXvFCBI",
            intents = 513,
            properties = properties
        };

        var identifyPayload = new DiscordPayload<Identify>(2, identify);
        SendDiscordData(clientWebSocket, identifyPayload).Wait();
        GetDiscordData(clientWebSocket).Wait();




        CloseSockets(clientWebSocket).Wait();

    }


    struct MessageComponent
    {

    }

    struct Identify
    {
        public string token;
        public int intents;
        public DiscordProperties properties;
    }

    struct DiscordProperties
    {
        [JsonProperty("$os")]
        public string os;

        [JsonProperty("$browser")]
        public string browser;

        [JsonProperty("$device")]
        public string device;
    }

    struct DiscordPayload<T>
    {
        public int op;
        public T d;

        public DiscordPayload(int v1, T v2)
        {
            op = v1;
            d = v2;
        }
    }
}
