using System.Text.Json.Serialization;

namespace StarWarsSPA.Core.Models
{
    public class Starship
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("manufacturer")]
        public string? Manufacturer { get; set; }

        [JsonPropertyName("cost_in_credits")]
        public string? CostInCredits { get; set; }

        [JsonPropertyName("crew")]
        public string? Crew { get; set; }

        [JsonPropertyName("passengers")]
        public string? Passengers { get; set; }

        [JsonPropertyName("starship_class")]
        public string? StarshipClass { get; set; }

        [JsonPropertyName("max_atmosphering_speed")]
        public string? MaxAtmospheringSpeed { get; set; }

        [JsonPropertyName("hyperdrive_rating")]
        public string? HyperdriveRating { get; set; }

        [JsonPropertyName("mglt")]
        public string? MGLT { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("pilots")]
        public List<string>? Pilots { get; set; }

        [JsonPropertyName("films")]
        public List<string>? Films { get; set; }
    }
}

