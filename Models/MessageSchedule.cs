using GMABot.Converters;
using Newtonsoft.Json;

namespace GMABot.Models
{
    // Wrapper for scheduling the message tasks
    struct MessageSchedule
    {
        [JsonConverter(typeof(TimeOnlyConverter))]
        public TimeOnly time;
        public DateTime dateTime;
        public string message;

        // TODO: Add global.channel as default
        public string channel;
    }
}
