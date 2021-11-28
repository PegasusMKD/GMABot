using GMABot.Converters;
using Newtonsoft.Json;

namespace GMABot.Models.Schedules
{
    // Wrapper for scheduling the message tasks
    internal class MessageSchedule : Schedule
    {
        public string message;
    }
}
