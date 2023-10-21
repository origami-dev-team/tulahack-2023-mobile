using DevExpress.Maui;
using DotNet.Meteor.HotReload.Plugin;
using SkiaSharp.Views.Maui.Controls.Hosting;

namespace Origami.ComixMaker;

public static class MauiProgram {
    public static MauiApp CreateMauiApp() {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .UseDevExpress()
            .UseSkiaSharp();

#if DEBUG
        builder.EnableHotReload();
#endif

        return builder.Build();
    }
}
