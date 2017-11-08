using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using RequestProcess.Attr;
using RequestProcess.Interface;
using RequestProcess.Utility;

namespace NhacVnDownloader
{
    [Site("nhac.vn")]
    public class NhacVnClient: AMusicDownloader
    {
        protected override string GetKey(string url)
        {
            return url;
        }

        protected override string GetXmlLink(string url)
        {
            return url;
        }

        protected override IEnumerable<string> GetMusicString(string xmlLink)
        {
            var content = Get(xmlLink);

            var regex = new Regex("playlist: (?<key>.+),");
            var value = regex.Match(content).Groups["key"]?.Value;
            var mediaModels = JsonConvert.DeserializeObject<List<MediaModel>>(value);
            foreach (var mediaModel in mediaModels)
            {
                var linkMusic = mediaModel.SourceModels.FirstOrDefault(x=>x.Label== "320K");
                var type = string.Empty;
                if (linkMusic != null)
                {
                    var split = linkMusic.File.Split('/').Last();
                    if (split.Contains(".mp3"))
                    {
                        type = "mp3";
                    }
                }
                var resuft = new
                {
                    Link = linkMusic?.File,
                    Title = mediaModel.MediaTitle,
                    Artist = mediaModel.MediaDesc,
                    Type = type
                };
                yield return resuft.ToJsonString();
            }
        }
    }
}
