using AngleSharp.Parser.Html;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using RLHelper.Morphemes;
using Android.Graphics;

namespace RLHelper
{
    class PageParser
    {

        public event Action<List<Morpheme>> OnNewMorphemeData;
        public event Action<Spell> OnNewSpellData;
        public event Action<string> OnNewInformation;

        HtmlParser parser = new HtmlParser();

        public async Task MorphemeParse(string response) {

            List<Morpheme> morphList = new List<Morpheme>();

            var document = await parser.ParseAsync(response);

            var t1 = document.QuerySelector("table#kuz_interpret");
            var t2 = t1.QuerySelector("table");
            var trS = t2.QuerySelectorAll("tr");   

            foreach (var tr in trS) {
                
                if (tr.QuerySelectorAll("td").Length < 2) { break; }

                var key = tr.QuerySelectorAll("td")[1];
                var value = tr.QuerySelectorAll("td")[0];

                if (key == null || value == null) { break; }

                if (key.TextContent == " — префикс (приставка)") {
                    morphList.Add(new Prefix(value.TextContent, Color.White));
                } else if (key.TextContent == " — корень") {
                    morphList.Add(new Root(value.TextContent, Color.Red));
                } else if (key.TextContent == " — суффикс") {
                    morphList.Add(new Suffix(value.TextContent, Color.Orange));
                } else if (key.TextContent == " — нулевое окончание") {
                    morphList.Add(new Ending(" ", Color.Green));
                } else if (key.TextContent == " — окончание") {
                    morphList.Add(new Ending(value.TextContent, Color.Purple));
                } else if (key.TextContent == " — постфикс") {
                    morphList.Add(new Postfix(value.TextContent, Color.Blue));
                } else if (key.TextContent == " — основа слова") {
                    morphList.Add(new WorldBase(value.TextContent, Color.Azure));
                }
            }

            OnNewMorphemeData?.Invoke(morphList);
        }

        public async Task SpellingParse(string response, string queryWord) {

            Spell sp;

            var document = await parser.ParseAsync(response);
            var div = document.QuerySelector("div#main");
            
            if (div.QuerySelectorAll("p").Length < 2) {
                sp = new Spell() {
                    word = queryWord,
                    spellsPos = new List<int>()
                };

                OnNewSpellData?.Invoke(sp);
                return;
            }

            var p = div.QuerySelectorAll("p")[1];
            string words = p.TextContent.Replace("Возможное правильное написание:", "").Replace(" ", "");
            string rightWord = words.Split(',')[0];

            sp = new Spell();

            sp.word = rightWord;
            sp.spellsPos = new List<int>();

            //Spell spell = new Spell() {
            //    word = rightWord,
            //    spellsPos = new List<int>()
            //};

            for (int i = 0; i < rightWord.Length; ++i) {
                if (queryWord.Length > i) {
                    if (rightWord[i] != queryWord[i]) {
                        sp.spellsPos.Add(i);            
                    }
                } 
            }

            OnNewSpellData?.Invoke(sp);
        }
    }
}