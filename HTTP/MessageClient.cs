using GMABot.Factories;
using GMABot.Models;
using GMABot.Timers;
using Newtonsoft.Json;
using System.Text;
using System.Timers;


namespace GMABot.Http
{
    class MessageClient
    {
        readonly HttpClient client = HttpClientFactory.GetHttpClient();

        // Timer constants
        const int checkTime =       1 * 60 * 1000;
        const int resetTime = 2 * 60 * 1000;

        // TODO: Check with higher amount of reset timer, if i'm correct, this should have a bug
        // The bug would be that it would skip the "activated" dailies because the reset will be
        // called 24h after starting the program
        //const int resetTime = 5 * 60 * 1000;

        // Discord
        readonly string defaultChannel;
        readonly MessageSchedule[] messages;

        // Timers
        private System.Timers.Timer resetTimer;
        private readonly List<MessageTimer> messageTimers = new();

        public MessageClient(Configuration configuration)
        {
            this.messages = configuration.messages;
            this.defaultChannel = configuration.channel;

            // Create reset timer
            CreateResetTimer();
        }

        public void Start()
        {
            foreach (MessageSchedule schedule in messages)
            {
                MessageTimer timer = new(checkTime, schedule.time);
                timer.Elapsed += new ElapsedEventHandler((e, v) =>
                    SendMessage(e as MessageTimer, schedule.message, schedule.channel)
                );
                messageTimers.Add(timer);
            }

            // Queue remaining messages of the day
            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
            messageTimers.FindAll(timer => timer.Time.CompareTo(currentTime) > 0).ForEach(timer => timer.Start());

            resetTimer.Start();

            // So we stall the main thread while the timers are ticking
            while (Console.ReadLine() != "q") ;
        }

        void SendMessage(MessageTimer? timer, string message, string channel)
        {
            if (timer == null) return;

            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
            if (!currentTime.IsBetween(timer.Time, timer.Time.AddMinutes(2))) return;

            Console.WriteLine($"[{DateTime.Now}] Sent message: {message}");

            var request = new HttpRequestMessage(HttpMethod.Post, HttpClientFactory.baseUri + $"/channels/{channel ?? defaultChannel}/messages");

            var messagePayload = new Message()
            {
                content = message
            };

            request.Content = new StringContent(JsonConvert.SerializeObject(messagePayload), Encoding.Unicode, "application/json");

            client.Send(request);
            timer.Stop();
        }

        void CreateResetTimer()
        {
            resetTimer = new(resetTime);
            resetTimer.Elapsed += new ElapsedEventHandler((e, v) => ResetTimers());
        }

        // Maybe use code given below for resetTimer (based on how we want to define the timer interval)
        // messageTimers.FindAll(timer => timer.Time.CompareTo(currentTime) > 0).ForEach(timer => timer.Start())
        void ResetTimers() => messageTimers.ForEach(m => m.Start());
    }

}
