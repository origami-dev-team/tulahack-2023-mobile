using RestSharp;

namespace Origami.Api;

public static class Repository {
    private static RestClient client;

    static Repository() {
        var options = new RestClientOptions("http://31.129.109.90:3000");
        client = new RestClient(options);
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
}
