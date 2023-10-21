using DevExpress.Maui.Controls;
using DevExpress.Maui.Editors;
using Origami.Api;

namespace Origami.ComixMaker;

public partial class ComixMakerPage : ContentPage {
    private ComixMakerViewModel viewModel;
    

    public ComixMakerPage() {
        viewModel = new ComixMakerViewModel();
    
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing() {
        base.OnAppearing();
        this.DoSafe(async() => {
            viewModel.PredefinedCharacters = await Repository.GetAllCharacters();
        });
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

    private void Prefedined_Clicked(object sender, EventArgs e) {
        viewModel.CharacterPickerBottomSheetState = BottomSheetState.HalfExpanded;
    }

    private void CollectionView_SelectionChanged(object? sender, SelectionChangedEventArgs e) {
        var item = e.CurrentSelection.FirstOrDefault();
        if (item == null)
            return;

        viewModel.FramesData.Last().BackgroundImagePath = (string)item;
        viewModel.CharacterPickerBottomSheetState = BottomSheetState.Hidden;
    }

    private async void Generate_Clicked(object sender, EventArgs e) {
        viewModel.CleanGeneratedDocuments();
        await viewModel.GenerateComix();
        TextField.Text = string.Empty;
        viewModel.CreateFrameBottomSheetState = BottomSheetState.Hidden;
    }
}
