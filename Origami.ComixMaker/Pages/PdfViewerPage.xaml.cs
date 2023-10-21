namespace Origami.ComixMaker;

public partial class PdfViewerPage : ContentPage {
    private string? filePath;
    public PdfViewerPage(string? filePath) {
        InitializeComponent();
        this.filePath = filePath;
    }

    protected override async void OnAppearing() {
        base.OnAppearing();
        if (string.IsNullOrEmpty(filePath))
            return;
        if (!filePath.StartsWith("http")) {
            BindingContext = filePath;
            return;
        }

        var path = Path.Combine(FileSystem.Current.AppDataDirectory, "downloaded.pdf");
        if (File.Exists(path))
            File.Delete(path);

        using (var client = new HttpClient()) {
            var stream = await client.GetStreamAsync(filePath);
            using (var fileStream = File.OpenWrite(path)) {
                await stream.CopyToAsync(fileStream);
            }
        }

        BindingContext = path;
    }
}

