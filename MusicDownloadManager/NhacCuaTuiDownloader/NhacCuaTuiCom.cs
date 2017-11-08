using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using RequestProcess.Attr;
using RequestProcess.Interface;
using RequestProcess.Utility;

namespace NhacCuaTuiDownloader
{
    [Site("www.nhaccuatui.com")]
    public class NhacCuaTuiCom : AMusicDownloader
    {
        protected override string GetKey(string url)
        {
            return Regex.Match(url, @"\.(?<key>[a-zA-Z0-9]+)\.html").Groups["key"].Value;
        }

        protected override string GetXmlLink(string url)
        {
            var content = Get(url);
            var regex = new Regex("xmlURL = \"(?<key>.+)\"");
            var match = regex.Match(content);
            return match.Groups["key"]?.Value;
        }

        protected override IEnumerable<string> GetMusicString(string xmlLink)
        {
            var content = GetContent(xmlLink);
            var document = new XmlDocument();
            document.LoadXml("<?xml version=\"1.0\" encoding=\"UTF-8\" ?>" + content);
            if (document.DocumentElement == null)
            {
                yield return null;
            }
            var selectNodes = document.DocumentElement.SelectNodes("//track");
            if (selectNodes == null)
                yield return null;
            foreach (XmlNode nodeItem in selectNodes)
            {
                if (nodeItem == null)
                    continue;

                string link = null, title = null, artist = null, type = null;
                var selectSourceNode = nodeItem.SelectSingleNode(".//location");
                //if (value.Equals("key3"))
                    //selectSourceNode = nodeItem.SelectSingleNode(".//bklink");
                if (selectSourceNode != null)
                {
                    link = selectSourceNode.InnerText;
                }
                var selectTitleNode = nodeItem.SelectSingleNode(".//title");
                if (selectTitleNode != null)
                {
                    title = selectTitleNode.InnerText;
                }
                var selectArtistNode = nodeItem.SelectSingleNode(".//creator");
                if (selectArtistNode != null)
                {
                    artist = selectArtistNode.InnerText;
                }
                if (string.IsNullOrEmpty(link))
                    continue;
                type = Regex.Match(link, @"\.(?<key>\w+$)").Groups["key"].Value;
                var resuft = new
                {
                    Link = link,
                    Title = title,
                    Artist = artist,
                    Type = type
                };
                yield return resuft.ToJsonString();
            }
        }
    }
}
