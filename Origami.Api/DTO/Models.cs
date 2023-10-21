using System.Text.Json.Serialization;

namespace Origami.Api;

public class AIGenerateBody {
    [JsonPropertyName("prompt")] public string? Prompt { get; set; }
}
