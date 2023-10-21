
namespace Origami.ComixMaker;

public static class ErrorExtensions {
    public static async Task DoSafe(this Page page, Func<Task> action) {
        try {
            await action();
        } catch (Exception e) {
            await page.DisplayAlert("Exception has been thrown", e.Message, "OK");
        }
    }
}