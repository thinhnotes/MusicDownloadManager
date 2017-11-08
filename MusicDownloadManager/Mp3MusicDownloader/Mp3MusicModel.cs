using Newtonsoft.Json;

namespace Mp3MusicDownloader
{
    public class Mp3MusicModel
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("artists_names")]
        public string ArtistsName { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("lyric")]
        public string Lyric { get; set; }

        [JsonProperty("thumbnail")]
        public string Thumbnail { get; set; }

        [JsonProperty("artist")]
        public Mp3ArtistModel Mp3ArtistModel { get; set; }

        [JsonProperty("source")]
        public Mp3SourceModel Mp3SourceModel { get; set; }
    }

    public class Mp3ArtistModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Cover { get; set; }

        public string Thumbnail { get; set; }
    }

    public class Mp3SourceModel
    {
        [JsonProperty("128")]
        public string Link128 { get; set; }

        [JsonProperty("320")]
        public string Link320 { get; set; }
    }
}
