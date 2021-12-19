using GMABot.Models;
using GMABot.Models.WebSocket;
using GMABot.Models.WebSocket.Events;
using GMABot.WebSocket;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Timers;

static class DiscordWebSocket
{
    // Default properties taken from some random site, maybe should change these in the future
    static readonly DiscordProperties properties =  new() {
        os = "linux",
        browser = "my_library",
        device = "my_library"
    };

    private static readonly Random random = new();
    private static System.Timers.Timer heartbeatTimer;

    // TODO: Clean up into a ".settings" file
    static readonly string token = "OTEzOTMzNTg5MzkyMDIzNTg0.YaFs-w.87wQwPP2ISCHroQqU-93kXvFCBI";
    // https://ziad87.net/intents/
    static readonly int intent = 513;

    public static string sessionId;
    static int latestSequenceNumber;

    static async Task<DiscordEventBase?> GetDiscordEvent(ClientWebSocket clientWebSocket, CancellationToken cancellationToken)
    {
        try
        {
            var buffer = new byte[1024];
            using(MemoryStream data = new(65536))
            {
                while (true)
                {
                    if(clientWebSocket.State != WebSocketState.Open) return null;
                    var received = await clientWebSocket.ReceiveAsync(buffer, cancellationToken);
                    if (received.MessageType != WebSocketMessageType.Close)
                        data.Write(buffer, 0, received.Count);

                    if (received.MessageType == WebSocketMessageType.Close || received.EndOfMessage)
                        break;
                }
                if (data == null)
                    return null;

                data.Position = 0;
                var jsonString = Encoding.UTF8.GetString(data.ToArray());
            
                // TODO: Add "interaction"/event specific deserialize
                var json = JsonConvert.DeserializeObject<DiscordEventBase>(jsonString);
                if(json == null) return null;
                json.json = jsonString;
                return json;
            }
        } catch (WebSocketException)
        {
            return null;
        }
    }

    static async Task SendDiscordEvent<T>(ClientWebSocket clientWebSocket, DiscordEventWrapper<T> payload)
    {
        CancellationToken cancellationToken = new();
        var payloadBuffer = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(payload));
        await clientWebSocket.SendAsync(new ArraySegment<byte>(payloadBuffer), WebSocketMessageType.Text, true, cancellationToken);
    }

    static async Task ConnectDiscordWebSocket(ClientWebSocket clientWebSocket, CancellationToken cancellationToken)
    {
        await clientWebSocket.ConnectAsync(new Uri("wss://gateway.discord.gg/?v=9&encoding=json"), cancellationToken);

        // Receive opCode 10 (client is connected)
        var result = await GetDiscordEvent(clientWebSocket, cancellationToken);
        var interval = JToken.Parse(result!.json!)!["d"]!.Value<int>("heartbeat_interval");
        
        heartbeatTimer = new System.Timers.Timer(interval);
        heartbeatTimer.Interval = interval;
        heartbeatTimer.Elapsed += new ElapsedEventHandler(async (e, v) =>
        {
            if(clientWebSocket.State != WebSocketState.Open) return;
            var payload = new DiscordEventWrapper<int>(1, 2);
            await SendDiscordEvent<int>(clientWebSocket, payload);
        });
        heartbeatTimer.Start();

        // Identify/login as the bot, and subscribe to specific events through the "intents" property
        await Identify(clientWebSocket);
    }

    static async Task Identify(ClientWebSocket clientWebSocket)
    {
        var identify = new IdentifyEvent()
        {
            token = token,
            intents = intent,
            properties = properties
        };

        var identifyPayload = new DiscordEventWrapper<IdentifyEvent>(2, identify);
        await SendDiscordEvent(clientWebSocket, identifyPayload);
    }

    static async Task CloseSockets(ClientWebSocket clientWebSocket)
    {
        if(clientWebSocket.State != WebSocketState.Open) return;

        await clientWebSocket.CloseOutputAsync(WebSocketCloseStatus.Empty, "", CancellationToken.None);
        if(clientWebSocket.CloseStatus != WebSocketCloseStatus.NormalClosure && clientWebSocket.CloseStatus != WebSocketCloseStatus.InternalServerError)
            await clientWebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);

        clientWebSocket.Abort();
    }

    public static async Task Start()
    {
        while(true) 
        { 
            CancellationToken cancellationToken = new();
            ClientWebSocket clientWebSocket = new ClientWebSocket();
            await ConnectDiscordWebSocket(clientWebSocket, cancellationToken);
            Console.WriteLine($"[{DateTime.Now}] Connected to Discord WebSocket.");
            // Skip last two events after connecting, they are only info about the bot becoming online
            while (true)
            {
                var result = await GetDiscordEvent(clientWebSocket, cancellationToken);

                if (clientWebSocket.State != WebSocketState.Open)
                {
                    Thread.Sleep(random.Next(1000, 5000));
                    break;
                }

                if (result?.op == 9)
                {
                    Thread.Sleep(random.Next(1000, 5000));
                    await Identify(clientWebSocket);
                }

                latestSequenceNumber = result?.s ?? latestSequenceNumber;
                EventDispatcher.Dispatch(result);
            }

            await CloseSockets(clientWebSocket);
        }
    }

    private static async Task Resume(ClientWebSocket clientWebSocket)
    {
        var resumeEvent = new ResumeEvent { seq = latestSequenceNumber, session_id = sessionId, token = token };
        var payload = new DiscordEventWrapper<ResumeEvent>(6, resumeEvent);
        await SendDiscordEvent(clientWebSocket, payload);
    }
}
