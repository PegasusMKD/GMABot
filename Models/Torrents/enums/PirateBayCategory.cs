namespace GMABot.Models.Torrents
{
    public enum PirateBayCategory
    {
        AUDIO = 100,
        VIDEO = 200,
        APPLICATIONS = 300,
        GAMES = 400,
        XXX = 500,
        OTHER = 600,
        DEFAULT = 0
    }

    static class PirateBayCategoryConverter
    {
        static Dictionary<PirateBayCategory, string> categories = new Dictionary<PirateBayCategory, string>
        {
            {PirateBayCategory.AUDIO, "audio" },
            {PirateBayCategory.VIDEO, "video" },
            {PirateBayCategory.APPLICATIONS, "applications" },
            {PirateBayCategory.GAMES, "games" },
            {PirateBayCategory.XXX, "xxx" },
            {PirateBayCategory.OTHER, "other" },
        };

        public static string GetText(PirateBayCategory type) => categories[type];

        public static PirateBayCategory GetType(string type) =>
            categories.FirstOrDefault(category => category.Value == type).Key;
    }
}
