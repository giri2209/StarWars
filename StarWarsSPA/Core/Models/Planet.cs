using System.Text.Json.Serialization;
namespace StarWarsSPA.Core.Models
{
    public class Planet
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("diameter")]
        public string? Diameter { get; set; }

        [JsonPropertyName("climate")]
        public string? Climate { get; set; }

        [JsonPropertyName("terrain")]
        public string? Terrain { get; set; }

        [JsonPropertyName("population")]
        public string? Population { get; set; }

        [JsonPropertyName("rotation_period")]
        public string? RotationPeriod { get; set; }

        [JsonPropertyName("orbital_period")]
        public string? OrbitalPeriod { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("films")]
        public List<string>? Films { get; set; }

        [JsonPropertyName("residents")]
        public List<string>? Residents { get; set; }
    }
}

