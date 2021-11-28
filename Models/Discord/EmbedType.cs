using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMABot.Models.Discord
{
    public enum EmbedType
    {
        RICH,
        IMAGE,
        VIDEO,
        GIFV,
        ARTICLE,
        LINK
    }

    static class EmbedConverter
    {
        static Dictionary<EmbedType, string> embedTypes = new Dictionary<EmbedType,string>
        {
            {EmbedType.RICH, "rich" },
            {EmbedType.IMAGE, "image" },
            {EmbedType.VIDEO, "video" },
            {EmbedType.GIFV, "gifv" },
            {EmbedType.ARTICLE, "article" },
            {EmbedType.LINK, "link" }
        };

        public static string GetText(EmbedType type) => embedTypes[type];

        public static EmbedType GetType(string type) =>
            embedTypes.FirstOrDefault(embed => embed.Value == type).Key;
    }
}
