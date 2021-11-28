using GMABot.Converters;
using Newtonsoft.Json;

namespace GMABot.Models.Schedules
{
    internal class Schedule
    {
        [JsonConverter(typeof(TimeOnlyConverter))]
        public TimeOnly time;
        public DateTime dateTime;

        // TODO: Add global.channel as default
        public string channel;
    }
}
