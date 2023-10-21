using DevExpress.Maui.Controls;
using DevExpress.Maui.Editors;

namespace Origami.ComixMaker;

public partial class ComixMakerPage : ContentPage {
    private ComixMakerViewModel viewModel;
    

    public ComixMakerPage() {
        viewModel = new ComixMakerViewModel();
    
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void Share_Clicked(object sender, EventArgs e) {
        if (viewModel.DocumentSource == null) {
            await DisplayAlert("Error", "Document is not generated", "OK");
            return;
        }
        await Share.Default.RequestAsync(new ShareFileRequest {
            Title = "Share pdf file",
            File = new ShareFile(viewModel.DocumentSource)
        });
    }

    private void Button_Clicked(object sender, EventArgs e) {
        viewModel.CreateFrameBottomSheetState = BottomSheetState.HalfExpanded;
        var test = new MultilineEdit();
    }

    private void Text_Changed(object? sender, EventArgs e) {
        var text = (sender as MultilineEdit)?.Text;
        if (text == null)
            return;

        viewModel.FramesData.Last().Text = text;
    }

    private async void Photo_Gallery(object sender, EventArgs e) {
        var result = await MediaPicker.PickPhotoAsync();
        if (result == null)
            return;

        viewModel.FramesData.Last().BackgroundImagePath = result.FullPath;
    }

    private async void Button_Clicked2(object sender, EventArgs e) {
        viewModel.CleanGeneratedDocuments();
        await viewModel.GenerateComix();
        TextField.Text = string.Empty;
        viewModel.CreateFrameBottomSheetState = BottomSheetState.Hidden;
    }
}
