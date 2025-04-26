using System.Text.Json.Serialization;

namespace StarWarsSPA.Core.Models
{
    public class Person
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        [JsonPropertyName("height")]
        public string? Height { get; set; }

        [JsonPropertyName("mass")]
        public string? Mass { get; set; }

        [JsonPropertyName("hair_color")]
        public string? HairColor { get; set; }

        [JsonPropertyName("skin_color")]
        public string? SkinColor { get; set; }

        [JsonPropertyName("eye_color")]
        public string? EyeColor { get; set; }

        [JsonPropertyName("birth_year")]
        public string? BirthYear { get; set; }

        [JsonPropertyName("gender")]
        public string? Gender { get; set; }

        [JsonPropertyName("url")]
        public string? Url { get; set; }

        [JsonPropertyName("films")]
        public List<string>? Films { get; set; }

        [JsonPropertyName("starships")]
        public List<string>? Starships { get; set; }

        [JsonPropertyName("species")]
        public List<string>? Species { get; set; }
    }

}


