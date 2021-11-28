using GMABot.Models.Schedules;

namespace GMABot.Models
{
    // The configuration file, more specifically, all the possible JSON properties
    struct Configuration
    {
        public string channel;
        public MessageSchedule[] messages;
        public HTMLSchedule[] htmlMessages;
    }
}
