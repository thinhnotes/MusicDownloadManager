using System;
using System.Text.RegularExpressions;
using THttpClient.Utility;

namespace ScribdDownloader
{
    public class ScribdClient : TWebRequest
    {
        private string GetKey(string url)
        {
            var regex = new Regex("/document/(?<key>\\d+)/");
            return regex.Match(url).Groups["key"]?.Value;
        }

        private string GetAccessToken(string url)
        {
            var content = Get(url);
            var regex = new Regex("\"access_key\":\"(?<key>[\\d\\w-]+)\"");
            return regex.Match(content).Groups["key"]?.Value;
        }

        public string GetUrlDocument(string url)
        {
            var key = GetKey(url);
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new Exception("Can not get key");
            }
            var accessToken = GetAccessToken(url);
            if (string.IsNullOrWhiteSpace(accessToken))
            {
                throw new Exception("Can not get access token");
            }

            return $"https://www.scribd.com/full/{key}?access_key={accessToken}";
        }
    }
}
