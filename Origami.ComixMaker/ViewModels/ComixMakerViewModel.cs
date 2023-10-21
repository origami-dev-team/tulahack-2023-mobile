using System.Drawing;
using DevExpress.Drawing;
using DevExpress.Pdf;
using Color = System.Drawing.Color;
using DXImage = DevExpress.Drawing.DXImage;
using System.ComponentModel;
using DevExpress.Maui.Controls;

namespace Origami.ComixMaker;

public class ComixMakerViewModel : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;

    public Action<string>? ProblemHandler { get; set; }

    private const string DocumentGeneratedName = "autostashfile";
    private const string FontFamilyName = "Comic Sans";
    private Rect pageViewPort;

    private BottomSheetState createFrameBottomSheetState = BottomSheetState.Hidden;
    public BottomSheetState CreateFrameBottomSheetState {
        get => createFrameBottomSheetState;
        set {
            createFrameBottomSheetState = value;
            OnPropertyChanged(nameof(CreateFrameBottomSheetState));
        }
    }

    private BottomSheetState pickerBottomSheetState = BottomSheetState.Hidden;
    public BottomSheetState PickerBottomSheetState {
        get => pickerBottomSheetState;
        set {
            pickerBottomSheetState = value;
            OnPropertyChanged(nameof(PickerBottomSheetState));
        }
    }

    private bool isBackgroundPickedInv = true;
    public bool IsBackgroundPickedInv {
        get => isBackgroundPickedInv;
        set {
            isBackgroundPickedInv = value;
            OnPropertyChanged(nameof(IsBackgroundPickedInv));
        }
    }

    private bool isCharacterPickedInv = true;
    public bool IsCharacterPickedInv {
        get => isCharacterPickedInv;
        set {
            isCharacterPickedInv = value;
            OnPropertyChanged(nameof(IsCharacterPickedInv));
        }
    }

    private List<string>? predefinedCharacters = null;
    public List<string>? PredefinedCharacters {
        get => predefinedCharacters;
        set {
            predefinedCharacters = value;
            OnPropertyChanged(nameof(PredefinedCharacters));
        }
    }

    private List<string>? predefinedBackgrounds = null;
    public List<string>? PredefinedBackgrounds {
        get => predefinedBackgrounds;
        set {
            predefinedBackgrounds = value;
            OnPropertyChanged(nameof(PredefinedBackgrounds));
        }
    }

    private string? documentSource = null;
    public string? DocumentSource {
        get => documentSource;
        set {
            documentSource = value;
            OnPropertyChanged(nameof(DocumentSource));
        }
    }

    public List<ComixFrameBuilder> FramesData { get; set; }

    public ComixMakerViewModel() {
        FramesData = new List<ComixFrameBuilder>();
        FramesData.Add(new ComixFrameBuilder());
        pageViewPort = new Rect(0, 0, 820, 1054);
    }


    public void CleanGeneratedDocuments() {
        var documents = Directory.GetFiles(FileSystem.Current.AppDataDirectory, $"*_{DocumentGeneratedName}.pdf");
        foreach (var document in documents)
            File.Delete(document);
    }
    public async Task GenerateComix() {
        var documentId = new Random().Next(0, 1000);
        var path = Path.Combine(FileSystem.Current.AppDataDirectory, $"{documentId}_{DocumentGeneratedName}.pdf");

        using (PdfDocumentProcessor processor = new PdfDocumentProcessor()) {
            processor.CreateEmptyDocument();
            foreach (var frameData in FramesData) {
                using (PdfGraphics graph = processor.CreateGraphics()) {
                    await DrawImage(graph, frameData.BackgroundImagePath);
                    await DrawImage(graph, frameData.PersonImagePath);
                    DrawText(graph, frameData);
                    processor.RenderNewPage(PdfPaperSize.Letter, graph);
                }
            }

            processor.SaveDocument(path);
            processor.CloseDocument();
        }

        if (!File.Exists(path))
            throw new FileNotFoundException(path);

        DocumentSource = path;
        FramesData.Add(new ComixFrameBuilder());
        ResetState();
    }

    public void ResetState() {
        FramesData.Last().PersonImagePath = string.Empty;
        FramesData.Last().BackgroundImagePath = string.Empty;
        FramesData.Last().Text = string.Empty;
        IsBackgroundPickedInv = true;
        IsCharacterPickedInv = true;
    }

    private async Task DrawImage(PdfGraphics graph, string imagePath) {
        if (string.IsNullOrEmpty(imagePath))
            return;

        using var stream = imagePath.StartsWith("http")
            ? await GetStreamFromUrl(imagePath)
            : new FileStream(imagePath, FileMode.Open, FileAccess.Read);

        if (stream is FileStream && (
                Path.GetExtension(imagePath) == ".jpg" ||
                Path.GetExtension(imagePath) == ".jpeg")
        )
            ProblemHandler?.Invoke("В данный момент jpeg файлы не обтображаются во встроенном предпросмотре. Мы можете открыть сгенерированный файл в любом pdf ридере.");

        using var image = DXImage.FromStream(stream);
        var pageCenter = new RectangleF(0, 0, (float)pageViewPort.Width, (float)pageViewPort.Height);
        graph.DrawImage(image, pageCenter);
    }
    private void DrawText(PdfGraphics graph, ComixFrameBuilder frameData) {
        if (string.IsNullOrEmpty(frameData.Text))
            return;

        using var whiteBrush = new DXSolidBrush(Color.White);
        var font = new DevExpress.Drawing.DXFont(FontFamilyName, 24, DXFontStyle.Bold);

        const float textMargin = 16f;
        var textPopupFrame = new RectangleF(0, (float)pageViewPort.Height * 0.8f, (float)pageViewPort.Width, (float)pageViewPort.Height * 0.2f);
        var textFrame = new RectangleF(textPopupFrame.Left + textMargin, textPopupFrame.Top + textMargin, textPopupFrame.Width - textMargin * 2, textPopupFrame.Height - textMargin * 2);

        graph.FillRectangle(new DXSolidBrush(Color.FromArgb(150, Color.Black)), textPopupFrame);
        graph.DrawString(frameData.Text, font, whiteBrush, textFrame);
    }


    private async Task<Stream> GetStreamFromUrl(string url) {
        using var client = new HttpClient();
        var bytes = await client.GetByteArrayAsync(url);
        return new MemoryStream(bytes);
    }

    private void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}