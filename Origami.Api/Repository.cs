using System.Text.Json;
using RestSharp;

namespace Origami.Api;

public static class Repository {
    private static RestClient client;
    private static HttpClient httpClient;

    static Repository() {
        var options = new RestClientOptions("http://31.129.109.90:3000");
        client = new RestClient(options);
        httpClient = new HttpClient();
    }

    public static async Task<List<string>?> GetAllCharacters(CancellationToken cancellationToken = default) {
        var request = new RestRequest("sprite/character");
        return await client.GetAsync<List<string>>(request, cancellationToken);
    }

    public static async Task<List<string>?> GetAllBackgrounds(CancellationToken cancellationToken = default) {
        var request = new RestRequest("sprite/background");
        return await client.GetAsync<List<string>>(request, cancellationToken);
    }

    public static async Task<List<string>?> GeneratBackgrounds(string query, CancellationToken cancellationToken = default) {
        var request = new RestRequest("sprite/background").AddBody(new AIGenerateBody() {
            Prompt = query
        });
        request.Timeout = 1000 * 60 * 5;
        return await client.PostAsync<List<string>?>(request, cancellationToken);
    }

    public static async Task<List<string>?> GeneratCharacters(string query, CancellationToken cancellationToken = default) {
        var request = new RestRequest("sprite/character").AddBody(new AIGenerateBody() {
            Prompt = query
        });
        request.Timeout = 1000 * 60 * 5;
        return await client.PostAsync<List<string>?>(request, cancellationToken);
    }

    public static async Task<List<Document>?> GetDocuments(CancellationToken cancellationToken = default) {
        var request = new RestRequest("comics");
        return await client.GetAsync<List<Document>>(request, cancellationToken);
    }

    public static async Task<Document?> LikeDocument(string id, CancellationToken cancellationToken = default) {
        var request = new RestRequest($"comics/{id}/like");
        return await client.PostAsync<Document>(request, cancellationToken);
    }

    public static async Task<Document?> UploadDocument(string title, string author, string filePath, CancellationToken cancellationToken = default) {
        var fileBytes = File.ReadAllBytes(filePath);
        var content = new MultipartFormDataContent();
        content.Add(new StringContent(title), "title");
        content.Add(new StringContent(author), "author");
        content.Add(new ByteArrayContent(fileBytes), "file", Path.GetFileName(filePath));
        var response = await httpClient.PostAsync("http://31.129.109.90:3000/comics", content, cancellationToken);
        var responseString = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<Document>(responseString);
    }
}
