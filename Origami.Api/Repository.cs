using RestSharp;

namespace Origami.Api;

public static class Repository {
    private static RestClient client;

    static Repository() {
        var options = new RestClientOptions("https://origami-team.site");
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

    // public static async Task<TodoItem?> CreateTodo(string title, string descr, CancellationToken cancellationToken = default) {
    //     var request = new RestRequest("todo").AddBody(new TodoCreateItem() {
    //         Title = title,
    //         Description = descr
    //     });
    //     return await client.PostAsync<TodoItem>(request, cancellationToken);
    // }
}
