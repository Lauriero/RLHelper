using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace RLHelper
{
    class PageParser
    {

        public event Action<MorphemeData> OnNewMorphemeData;
        public event Action<List<Spell>> OnNewSpellData;

        HtmlParser parser = new HtmlParser();

        public void MorphemeParse(string response) {

            MorphemeData data = new MorphemeData();

            var document = parser.Parse(response);

            var t1 = document.QuerySelector("table#kuz_interpret");
            var t2 = t1.QuerySelector("table");
            var trS = t2.QuerySelectorAll("tr");

            foreach (var tr in trS) {
                if (tr.QuerySelectorAll("td").Length < 2) { return; }

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

            OnNewMorphemeData?.Invoke(data);
        }

        public async Task SpellingParse(string response, string queryWord) {

            List<Spell> spells = new List<Spell>();

            var document = await parser.ParseAsync(response);

            var div = document.QuerySelector("div#main");

            if (div.QuerySelectorAll("p").Length < 2) {
                spells.Add(new Spell() { word = queryWord, spellsPos = new List<int>() });
                OnNewSpellData?.Invoke(spells);
                return;
            }

            var p = div.QuerySelectorAll("p")[1];

            string words = p.TextContent.Replace("Возможное правильное написание:", "").Replace(" ", "");

            foreach (string word in words.Split(',')) {
                Spell spell = new Spell() {
                    word = word,
                    spellsPos = new List<int>()
                };

                for (int i = 0; i < word.Length; ++i) {
                    if (queryWord.Length > i) {

                        if (word[i] != queryWord[i]) {
                            spell.spellsPos.Add(i);            
                        }
                    } 
                }

                spells.Add(spell);
            }

            

            OnNewSpellData?.Invoke(spells);

        }
    }
}