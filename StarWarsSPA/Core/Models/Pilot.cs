using System.Text.Json.Serialization;

namespace StarWarsSPA.Core.Models
{
    public class Pilot
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("Url")]
        public string? Url { get; set; }
    }
}

