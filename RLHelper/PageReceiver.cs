using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.IO;
using System.Collections;
using System.Collections.Generic;

namespace RLHelper
{
    class PageReceiver
    {

        private HttpClient _client = new HttpClient();

        string morphemeResource = "https://udarenieru.ru/index.php?word=on&morph_word=";
        string spellsResource = "https://bugaga.net.ru/orfografija/";

        public event Action<string> OnMorphemePageParse;
        public event Action<string> OnSpellsPageParse;
        public event Action<Exception> OnExceptionThrow;

        public async Task createHttpGetRequestAsync(string w)
        {
            try {

                WebRequest req = WebRequest.Create(morphemeResource + w);
                WebResponse resp = await req.GetResponseAsync();

                Stream stream = resp.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string responseString = await sr.ReadToEndAsync();

                sr.Close();

                OnMorphemePageParse?.Invoke(responseString);

            } catch (Exception e) {
                OnExceptionThrow?.Invoke(e);
            }
        }

        public async Task createHttpPostRequestAsync(string w) {
            try {

                Dictionary<string, string> dict = new Dictionary<string, string>() {
                    {"text", w}
                };

                FormUrlEncodedContent content = new FormUrlEncodedContent(dict);
                HttpResponseMessage response = await _client.PostAsync(spellsResource, content);

                string result = await response.Content.ReadAsStringAsync();

                OnSpellsPageParse?.Invoke(result);

            } catch (Exception e) {
                OnExceptionThrow?.Invoke(e);
            }
        }
    }
}