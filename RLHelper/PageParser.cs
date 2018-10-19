using AngleSharp.Parser.Html;
using System;

namespace RLHelper
{
    class PageParser
    {

        public event Action<string> OnNewData;

        HtmlParser parser = new HtmlParser();

        public void Parse(string response) {
            var document = parser.Parse(response);

            var t1 = document.QuerySelector("table#kuz_interpret");
            var t2 = t1.QuerySelector("table");
            var trS = t2.QuerySelectorAll("tr");

            foreach (var tr in trS) {
                var key = tr.QuerySelectorAll("td")[1];
                var value = tr.QuerySelectorAll("td")[0];

                if (key.TextContent == " — корень") {
                    OnNewData?.Invoke(value.TextContent);
                }
            }
        }
    }
}