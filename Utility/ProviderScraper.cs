/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace VFBlazor6._0.Utility
{
    internal static class ProviderScraper
    {

        internal static void test()
        {
            HtmlWeb web = new HtmlWeb();
            HtmlDocument doc = web.Load("https://en.wikipedia.org/wiki/Greece");

            var names = doc.DocumentNode.SelectNodes("/html/body/div[3]/div[2]/div/div/div[1]/nav/div[2]");
            foreach (var item in names)
            {
                Console.WriteLine(item.InnerText);
            }
        }

    }
}
*/