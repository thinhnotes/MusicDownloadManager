using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RequestProcess;
using RequestProcess.Attr;
using RequestProcess.HtmlAgiliPack;
using RequestProcess.Interface;
using RequestProcess.Utility;

namespace Mp3MusicDownloader
{
    [Site("mp3.zing.vn")]
    public class Mp3Music : AMusicDownloader
    {
        protected override string GetKey(string url)
        {
            return Regex.Match(url, "/(?<key>.{8}).html").Groups["key"].Value;
        }

        protected override string GetXmlLink(string url)
        {
            var content = GetContent(url);
            var xmlLink = Regex.Match(content, "data-xml=\"(?<link>[^\"]+)").Groups["link"].Value;
            if (!xmlLink.StartsWith("http"))
            {
                xmlLink = "https://mp3.zing.vn/xhr" + xmlLink;
            }
            return xmlLink;
        }

        protected override IEnumerable<string> GetMusicString(string xmlLink)
        {
            var content = GetContent(xmlLink);

            var sObject = JObject.Parse(content).SelectToken("data");

            if (sObject is JObject && sObject["items"] != null)
            {
                sObject = sObject["items"];
            }

            if (sObject is JObject && sObject["item"] != null)
            {
                sObject = sObject["item"];
            }

            var mp3MusicModels = new List<Mp3MusicModel>();

            if (sObject is JArray)
            {
                mp3MusicModels = JsonConvert.DeserializeObject<List<Mp3MusicModel>>(sObject.ToString());
            }
            else
            {
                var mp3MusicModel = JsonConvert.DeserializeObject<Mp3MusicModel>(sObject.ToString());
                mp3MusicModels.Add(mp3MusicModel);
            }

            foreach (var mp3MusicModel in mp3MusicModels)
            {
                string type = "mp3";
                if (mp3MusicModel != null && mp3MusicModel.Type == "audio")
                {
                    type = "mp3";
                }
                var resuft = new
                {
                    Link = mp3MusicModel?.Mp3SourceModel?.Link128,
                    Title = mp3MusicModel?.Title,
                    Artist = mp3MusicModel?.Mp3ArtistModel?.Name,
                    Type = type
                };
                if (!string.IsNullOrWhiteSpace(resuft.Link))
                    yield return resuft.ToJsonString();
            }
        }
    }
}
