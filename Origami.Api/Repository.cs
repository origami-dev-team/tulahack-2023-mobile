using RestSharp;

namespace Origami.Api;

public static class Repository {
    private static RestClient client;

    static Repository() {
        var options = new RestClientOptions("https://origami-team.site");
        client = new RestClient(options);
    }

    public static async Task<List<TodoItem>?> GetAllTodos(CancellationToken cancellationToken = default) {
        var request = new RestRequest("todo");
        return await client.GetAsync<List<TodoItem>>(request, cancellationToken);
    }

    public static async Task<TodoItem?> CreateTodo(string title, string descr, CancellationToken cancellationToken = default) {
        var request = new RestRequest("todo").AddBody(new TodoCreateItem() {
            Title = title,
            Description = descr
        });
        return await client.PostAsync<TodoItem>(request, cancellationToken);
    }
}
