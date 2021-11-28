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

        public static string ParseHtmlText(string url)
        {
            var htmlDocument = new HtmlDocument();
            htmlDocument.LoadHtml(GetHtml(url));
            return htmlDocument.GetElementbyId("text").InnerText;
        }

        static string GetHtml(string url) =>
            httpClient.GetStringAsync(url).Result;
    }
}
