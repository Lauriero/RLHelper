using AngleSharp.Parser.Html;
using System;

namespace RLHelper
{
    class PageParser
    {

        public event Action<MorphemeData> OnNewData;

        HtmlParser parser = new HtmlParser();

        public void Parse(string response) {

            MorphemeData data = new MorphemeData();

            var document = parser.Parse(response);

            var t1 = document.QuerySelector("table#kuz_interpret");
            var t2 = t1.QuerySelector("table");
            var trS = t2.QuerySelectorAll("tr");

            foreach (var tr in trS) {
                var key = tr.QuerySelectorAll("td")[1];
                var value = tr.QuerySelectorAll("td")[0];

                if (key == null || value == null) { return; }

                if (key.TextContent == "  — префикс (приставка)") {
                    if (data.prefixFirst == "") { data.prefixFirst = value.TextContent; }
                    else { data.prefixSecond = value.TextContent; }
                } else if (key.TextContent == " — корень") {
                    data.root = value.TextContent;       
                } else if (key.TextContent == " — суффикс") {
                    if (data.suffixFirst == "") { data.suffixFirst = value.TextContent; }
                    else { data.suffixSecond = value.TextContent; }
                } else if (key.TextContent == " — нулевое окончание") {
                    data.ending = "null";    
                } else if (key.TextContent == " — окончание") {
                    data.ending = value.TextContent;     
                } else if (key.TextContent == " — основа слова") {
                    data.basis = value.TextContent;
                }
            }

            OnNewData?.Invoke(data);
        }
    }
}