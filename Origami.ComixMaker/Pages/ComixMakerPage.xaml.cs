using DevExpress.Maui.CollectionView;
using DevExpress.Maui.Controls;
using DevExpress.Maui.Core;
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

    protected override async void OnAppearing() {
        base.OnAppearing();
        await this.DoSafe(async() => {
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

    private void AICharacter_Clicked(object sender, EventArgs e) {
        viewModel.AICharacterBottomSheetState = BottomSheetState.HalfExpanded;
    }
    private void AIBackground_Clicked(object sender, EventArgs e) {
        viewModel.AIBackgroundBottomSheetState = BottomSheetState.HalfExpanded;
    }
    private void Publish_Clicked(object sender, EventArgs e) {
        viewModel.PublishBottomSheetState = BottomSheetState.HalfExpanded;
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

    private void AIBackgroundView_SelectionChanged(object? sender, CollectionViewSelectionChangedEventArgs e) {
        var item = e.AddedItems.FirstOrDefault();
        if (item == null)
            return;

        viewModel.FramesData.Last().BackgroundImagePath = (string)item;
        viewModel.IsBackgroundPickedInv = false;
        viewModel.AIBackgroundBottomSheetState = BottomSheetState.Hidden;
    }
    private void AICharacterView_SelectionChanged(object? sender, CollectionViewSelectionChangedEventArgs e) {
        var item = e.AddedItems.FirstOrDefault();
        if (item == null)
            return;

        viewModel.FramesData.Last().PersonImagePath = (string)item;
        viewModel.IsCharacterPickedInv = false;
        viewModel.AICharacterBottomSheetState = BottomSheetState.Hidden;
    }

    private async void Generate_Clicked(object sender, EventArgs e) {
        var button = (DXButton)sender;
        button.IsEnabled = false;
        viewModel.CleanGeneratedDocuments();
        await viewModel.GenerateComix();
        TextField.Text = string.Empty;
        viewModel.CreateFrameBottomSheetState = BottomSheetState.Hidden;
        button.IsEnabled = true;
    }

    private async void AIBGenerate_Clicked(object sender, EventArgs e) {
        var button = (DXButton)sender;
        button.IsEnabled = false;
        button.Content = "Генерация...";
        await this.DoSafe(async () => {
            if (string.IsNullOrEmpty(AITextField1.Text))
                throw new Exception("Text is empty");

            var items = await Repository.GeneratBackgrounds(AITextField1.Text);
            AIBackgroundView.ItemsSource = items;
        });

        button.IsEnabled = true;
        button.Content = "Создать фон";
    }

    private async void AICGenerate_Clicked(object sender, EventArgs e) {
        var button = (DXButton)sender;
        button.IsEnabled = false;
        button.Content = "Генерация...";
        await this.DoSafe(async () => {
            if (string.IsNullOrEmpty(AITextField2.Text))
                throw new Exception("Text is empty");

            var items = await Repository.GeneratCharacters(AITextField2.Text);
            AICharacterView.ItemsSource = items;
        });

        button.IsEnabled = true;
        button.Content = "Создать персонажа";
    }

    private enum PickerType {
        Character,
        Background
    }
}
