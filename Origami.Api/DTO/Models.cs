using System.Text.Json.Serialization;

namespace Origami.Api;

public class AIGenerateBody {
    [JsonPropertyName("prompt")] public string? Prompt { get; set; }
}

public class UploadDocumentBody {
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("autor")] public string? Autor { get; set; }
}

public class Document {
    [JsonPropertyName("id")] public string? Id { get; set; }
    [JsonPropertyName("title")] public string? Title { get; set; }
    [JsonPropertyName("author")] public string? Author { get; set; }
    [JsonPropertyName("url")] public string? Url { get; set; }
    [JsonPropertyName("likes")] public int Likes { get; set; }
    [JsonPropertyName("preview")] public string? Preview { get; set; }
}