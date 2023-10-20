using System.Text.Json.Serialization;

namespace Origami.Api;

public class TodoItem {
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("title")] public string? Title { get; set; }
}

public class TodoCreateItem {
    [JsonPropertyName("description")] public string? Description { get; set; }
    [JsonPropertyName("title")] public string? Title { get; set; }
}