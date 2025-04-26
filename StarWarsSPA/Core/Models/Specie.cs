using System.Text.Json.Serialization;

namespace StarWarsSPA.Core.Models
{
    public class Specie
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("classification")]
        public string? Classification { get; set; }

        [JsonPropertyName("designation")]
        public string? Designation { get; set; }

        [JsonPropertyName("average_height")]
        public string? AverageHeight { get; set; }

        [JsonPropertyName("average_lifespan")]
        public string? AverageLifespan { get; set; }

        [JsonPropertyName("skin_colors")]
        public string? SkinColors { get; set; }

        [JsonPropertyName("eye_colors")]
        public string? EyeColors { get; set; }

        [JsonPropertyName("hair_colors")]
        public string? HairColors { get; set; }

        [JsonPropertyName("language")]
        public string? Language { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("films")]
        public List<string>? Films { get; set; }

        [JsonPropertyName("people")]
        public List<string>? People { get; set; }
    }
}

