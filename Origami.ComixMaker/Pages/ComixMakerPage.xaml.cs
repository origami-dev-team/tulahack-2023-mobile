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

    private async void Button_Clicked3(object sender, EventArgs e) {
        var result = await MediaPicker.PickPhotoAsync();
        if (result == null)
            return;

        viewModel.FramesData.Last().BackgroundImagePath = result.FullPath;
    }

    private async void Button_Clicked2(object sender, EventArgs e) {
        viewModel.CleanGeneratedDocuments();
        await viewModel.GenerateComix();
    }
}
