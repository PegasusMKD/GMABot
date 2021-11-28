using GMABot.Converters;
using GMABot.Models.Discord;
using Newtonsoft.Json;

namespace GMABot.Models.Schedules
{
    internal class Schedule
    {
        [JsonConverter(typeof(TimeOnlyConverter))]
        public TimeOnly time;
        public DateTime? dateTime;

        public string? channel;

        public string? title;
        public FormatType type = FormatType.MESSAGE;

        [JsonConverter(typeof(EmbedJsonConverter))]
        public EmbedType embedType = EmbedType.RICH;
    }
}
