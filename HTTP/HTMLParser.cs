using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GMABot.HTTP
{
    static internal class HTMLParser
    {
        private static readonly HttpClient httpClient = new HttpClient();

        // TODO: Transform this into ParseHtmlText(url, params string[] tags), and then return a list of all the innerText of the tags
        public static string GetDailyHoroscopeText(string url, string tag)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(GetHtml(url));
            return htmlDocument.GetElementbyId(tag).InnerText;
        }

        static string GetHtml(string url) =>
            httpClient.GetStringAsync(url).Result;
    }
}
