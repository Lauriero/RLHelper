using System;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.IO;


namespace RLHelper
{
    class PageReceiver
    {

        string baseQueryString = "https://udarenieru.ru/index.php?word=on&morph_word=";

        public event Action<string> OnPageParse;
        public event Action<Exception> OnExceptionThrow;

        public void Start(string word) {
            createHttpRequestAsync(word);
        }

        private void createHttpRequestAsync(string w)
        {
            try {

                WebRequest req = WebRequest.Create(baseQueryString + w);
                WebResponse resp = req.GetResponse();

                Stream stream = resp.GetResponseStream();
                StreamReader sr = new StreamReader(stream);
                string responseString = sr.ReadToEnd();

                sr.Close();

                OnPageParse?.Invoke(responseString);

            } catch (Exception e) {
                OnExceptionThrow?.Invoke(e);
            }
        }
    }
}