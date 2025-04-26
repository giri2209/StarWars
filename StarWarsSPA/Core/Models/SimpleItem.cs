using System.Text.Json.Serialization;

namespace StarWarsSPA.Core.Models
{
    public class SimpleItem
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }
    }
}

