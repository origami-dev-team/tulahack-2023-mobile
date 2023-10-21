using DevExpress.Maui.Controls;
using DevExpress.Maui.Editors;
using Origami.Api;

namespace Origami.ComixMaker;

public partial class ComixMakerPage : ContentPage {
    private ComixMakerViewModel viewModel;
    private PickerType pickerType;
    

    public ComixMakerPage() {
        viewModel = new ComixMakerViewModel();
        viewModel.ProblemHandler = async (message) => {
            await DisplayAlert("Warning", message, "OK");
        };
    
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override void OnAppearing() {
        base.OnAppearing();
        this.DoSafe(async() => {
            viewModel.PredefinedCharacters = await Repository.GetAllCharacters();
            viewModel.PredefinedBackgrounds = await Repository.GetAllBackgrounds();
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
        viewModel.IsBackgroundPickedInv = false;
    }

    private void Prefedined_Clicked(object sender, EventArgs e) {
        pickerType = PickerType.Character;
        Picker.ItemsSource = viewModel.PredefinedCharacters;
        viewModel.PickerBottomSheetState = BottomSheetState.HalfExpanded;
    }

    private void Prefedined_Clicked2(object sender, EventArgs e) {
        pickerType = PickerType.Background;
        Picker.ItemsSource = viewModel.PredefinedBackgrounds;
        viewModel.PickerBottomSheetState = BottomSheetState.HalfExpanded;
    }

    private void Reset_Clicked(object sender, EventArgs e) {
        TextField.Text = string.Empty;
        viewModel.ResetState();
    }

    private void CollectionView_SelectionChanged(object? sender, SelectionChangedEventArgs e) {
        var item = e.CurrentSelection.FirstOrDefault();
        if (item == null)
            return;

        if (pickerType == PickerType.Character) {
            viewModel.FramesData.Last().PersonImagePath = (string)item;
            viewModel.IsCharacterPickedInv = false;
        } else {
            viewModel.FramesData.Last().BackgroundImagePath = (string)item;
            viewModel.IsBackgroundPickedInv = false;
        }
        viewModel.PickerBottomSheetState = BottomSheetState.Hidden;
    }

    private async void Generate_Clicked(object sender, EventArgs e) {
        viewModel.CleanGeneratedDocuments();
        await viewModel.GenerateComix();
        TextField.Text = string.Empty;
        viewModel.CreateFrameBottomSheetState = BottomSheetState.Hidden;
    }

    private enum PickerType {
        Character,
        Background
    }
}
