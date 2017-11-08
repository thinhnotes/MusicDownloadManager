using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using RequestProcess.Utility;

namespace RequestProcess
{
    public enum TypeRequest
    {
        Normal,
        Json
    }

    public class TWebRequest
    {
        private string _userAngent;

        protected TWebRequest()
        {
            CookieCollection = new CookieCollection();
        }

        protected TWebRequest(CookieCollection cookie)
        {
            CookieCollection = cookie;
        }

        protected TWebRequest(string fileName):this()
        {
            string text = File.ReadAllText(fileName);
            var deserializeJsonAs = text.DeserializeJsonAs<IEnumerable<Dictionary<string, object>>>();
            var cookieCollection = new CookieCollection();
            foreach (var jsonA in deserializeJsonAs)
            {
                var cookie = new Cookie()
                {
                    Domain = (string) jsonA["domain"],
                    HttpOnly = (bool) jsonA["hostOnly"],
                    Name = (string) jsonA["name"],
                    Path = (string) jsonA["path"],
                    Value = (string) jsonA["value"],
                    Secure = (bool) jsonA["secure"],
                };
                cookieCollection.Add(cookie);
            }
            CookieCollection = cookieCollection;
        }

        protected CookieCollection CookieCollection { get; set; }
        protected bool AutoRedirect { get; set; }
        protected string Location { get; set; }
        protected string Referer { get; set; }

        protected TypeRequest Type { get; set; }

        protected string UserAngent
        {
            get
            {
                return _userAngent ??
                       "Mozilla/5.0 (Windows NT 6.3; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/38.0.2125.104 Safari/537.36";
            }
            set { _userAngent = value; }
        }

        protected bool Gzip { get; set; }
        private Uri Uri { get; set; }

        private HttpWebResponse GetResponse(string url, string postData = null, string method = "GET")
        {
            Uri = new Uri(url);
            HttpWebRequest request = InitRequest();
            if (method.Equals("POST", StringComparison.OrdinalIgnoreCase))
            {
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                if (postData != null)
                {
                    byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                    request.ContentLength = byteArray.Length;
                    Stream dataStream = request.GetRequestStream();
                    dataStream.Write(byteArray, 0, byteArray.Length);
                    dataStream.Close();
                }
                else
                    request.ContentLength = 0;
            }
            return (HttpWebResponse) request.GetResponse();
        }

        private Stream GetStream(string url, string postData = null, string method = "GET")
        {
            HttpWebResponse response = GetResponse(url, postData, method);

            if (AutoRedirect == false)
            {
                if (response.StatusCode == HttpStatusCode.Redirect || response.StatusCode == HttpStatusCode.Found)
                {
                    // Do something...
                    Location = response.Headers["Location"];
                }
            }
            CookieCollection.Add(response.Cookies);
            Stream responseStream = response.GetResponseStream();
            return responseStream;
        }

        private string GetContent(string url, string postData = null, string method = "GET")
        {
            using (Stream stream = GetStream(url, postData, method))
            {
                using (var sr = new StreamReader(stream))
                {
                    return sr.ReadToEnd();
                }
            }
        }

        protected string Get(string url, bool autoRedirect = false)
        {
            AutoRedirect = autoRedirect;
            return GetContent(url);
        }

        protected string Post(string url, string data, TypeRequest type = TypeRequest.Normal, bool autoRedirect = false)
        {
            AutoRedirect = autoRedirect;
            Type = type;
            return GetContent(url, data, "POST");
        }

        public string Post(string url, params KeyValuePair<string,string>[] data)
        {
            string dataOuput = data.Aggregate("123", (current, keyValuePair) => current + string.Format("{0}={1}", keyValuePair.Key, keyValuePair.Value));
            return Post(url, dataOuput);
        }

        private HttpWebRequest InitRequest()
        {
            var request = (HttpWebRequest) WebRequest.Create(Uri);
            SetInitRequest(request);
            if (request.CookieContainer == null)
                request.CookieContainer = new CookieContainer();
            request.CookieContainer = new CookieContainer();
            if(CookieCollection!=null)
                request.CookieContainer.Add(CookieCollection);
            if (Type == TypeRequest.Json)
            {
                request.Headers.Add("X-Requested-With", "XMLHttpRequest");
            }
            return request;
        }

        protected void SetInitRequest(HttpWebRequest request)
        {
            request.AllowAutoRedirect = AutoRedirect;
            request.Headers.Add(HttpRequestHeader.AcceptLanguage, "en,vi;q=0.8,en-US;q=0.6");
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            request.Referer = Referer;
            request.UserAgent = UserAngent;
            request.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate |
                                             DecompressionMethods.None;
        }
    }
}