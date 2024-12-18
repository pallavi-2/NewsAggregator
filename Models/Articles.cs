using Newtonsoft.Json;

namespace NewsAggregator.Models
{
    public class RootResponse
    {
        public string Status { get; set; }
        [JsonProperty("news")]
        public List<Articles> Articles { get; set; }
    }
    public class Articles
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("author")]
        public string Author { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("category")]
        public string[] Category { get; set; }

        [JsonProperty("image")]
        public string Image { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("language")]
        public string Language { get; set; }

        [JsonProperty("published")]
        public DateTime PublishedAt { get; set; }
    }
}
