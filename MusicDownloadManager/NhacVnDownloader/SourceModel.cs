using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace NhacVnDownloader
{
    public class MediaModel
    {
        public string MediaId { get; set; }

        [JsonProperty("media_title")]
        public string MediaTitle { get; set; }

        [JsonProperty("media_desc")]
        public string MediaDesc { get; set; }

        [JsonProperty("sources")]
        public List<SourceModel> SourceModels { get; set; }
    }

    public class SourceModel
    {
        public string File { get; set; }
        public string Label { get; set; }
    }
}
