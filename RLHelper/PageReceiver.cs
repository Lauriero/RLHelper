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

        string baseQueryString = "https://bugaga.net.ru/orfografija/";

        public event Action<string> OnPageParse;
        public event Action<Exception> OnExceptionThrow;

        public void StartGet(string word) {
            Task.Run(() => createHttpGetRequestAsync(word));
        }

        public void StartPost(string text)
        {
            Task.Run(() => createHttpPostRequestAsync(text));
        }

        public async Task createHttpGetRequestAsync(string w)
        {
            try {

                WebRequest req = WebRequest.Create(baseQueryString + w);
                WebResponse resp = await req.GetResponseAsync();

                Stream stream = resp.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string responseString = await sr.ReadToEndAsync();

                sr.Close();

                OnPageParse?.Invoke(responseString);

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
                HttpResponseMessage response = await _client.PostAsync(baseQueryString, content);

                string result = await response.Content.ReadAsStringAsync();   
                
                OnPageParse?.Invoke(result);

            } catch (Exception e) {
                OnExceptionThrow?.Invoke(e);
            }
        }
    }
}