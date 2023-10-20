using DevExpress.Maui;
using DotNet.Meteor.HotReload.Plugin;

namespace Origami.ComixMaker;

public static class MauiProgram {
    public static MauiApp CreateMauiApp() {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseDevExpress();

#if DEBUG
        builder.EnableHotReload();
#endif

        return builder.Build();
    }
}
