
namespace Origami.ComixMaker;

public static class ErrorExtensions {
    public static void DoSafe(this Page page, Action action) {
        try {
            action();
        } catch (Exception e) {
            page.DisplayAlert("Exception has been thrown", e.Message, "OK");
        }
    }
}