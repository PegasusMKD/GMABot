using GMABot.Factories;
using GMABot.HTTP;
using GMABot.Models;
using GMABot.Models.Schedules;
using GMABot.Timers;
using Newtonsoft.Json;
using System.Text;
using System.Timers;


namespace GMABot.Http
{
    class MessageScheduler
    {
        readonly HttpClient client = HttpClientFactory.GetHttpClient();

        // Timer constants
        const int checkTime =      1 * 60 * 1000;
        const int resetTime = 24 * 60 * 60 * 1000;

        // TODO: Check with higher amount of reset timer, if i'm correct, this should have a bug
        // The bug would be that it would skip the "activated" dailies because the reset will be
        // called 24h after starting the program
        //const int resetTime = 5 * 60 * 1000;

        // Discord
        readonly string defaultChannel;
        readonly MessageSchedule[] messages;
        readonly HTMLSchedule[] htmlSchedules;

        // Timers
        private System.Timers.Timer resetTimer;
        private readonly List<MessageTimer> scheduleTimers = new();

        public MessageScheduler(Configuration configuration)
        {
            this.messages = configuration.messages;
            this.htmlSchedules = configuration.htmlMessages;
            this.defaultChannel = configuration.channel;

            // Create reset timer
            CreateResetTimer();
        }

        public void Start()
        {
            ScheduleMessages<MessageSchedule>(messages, (schedule, timer) =>
                {
                    var message = DiscordMessageFactory.CreateMessage(schedule, schedule.message);
                    DiscordHttpClient.SendTimerMessage(timer, message, schedule.channel ?? defaultChannel);
                }
            );

            ScheduleMessages<HTMLSchedule>(htmlSchedules, (schedule, timer) =>
                {
                    var message = DiscordMessageFactory.CreateMessage(schedule, HTMLParser.GetDailyHoroscopeText(schedule.url, "text"));
                    DiscordHttpClient.SendTimerMessage(timer, message, schedule.channel ?? defaultChannel);
                }
            );

            // Queue remaining messages of the day
            TimeOnly currentTime = TimeOnly.FromDateTime(DateTime.Now);
            scheduleTimers.FindAll(timer => timer.Time.CompareTo(currentTime) > 0).ForEach(timer => timer.Start());

            resetTimer.Start();
        }

        void ScheduleMessages<T>(T[] messages, Action<T, MessageTimer> action) where T: Schedule
        {
            foreach (T schedule in messages)
            {
                MessageTimer timer = new(checkTime, schedule.time);
                timer.Elapsed += new ElapsedEventHandler((e, v) =>
                    action(schedule, (e as MessageTimer)!)
                );
                scheduleTimers.Add(timer);
            }
        }

        void CreateResetTimer()
        {
            resetTimer = new(resetTime);
            resetTimer.Elapsed += new ElapsedEventHandler((e, v) => ResetTimers());
        }

        // Maybe use code given below for resetTimer (based on how we want to define the timer interval)
        // messageTimers.FindAll(timer => timer.Time.CompareTo(currentTime) > 0).ForEach(timer => timer.Start())
        void ResetTimers() => scheduleTimers.ForEach(m => m.Start());
    }

}
