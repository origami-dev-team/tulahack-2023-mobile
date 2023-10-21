using DevExpress.Maui.CollectionView;
using DevExpress.Maui.Core;
using Origami.Api;

namespace Origami.ComixMaker;

public partial class MainPage : ContentPage {
    private ComixMakerPage comixMakerPage = new ComixMakerPage();
    private ProfilePage profilePage = new ProfilePage();
    private MainViewModel viewModel = new MainViewModel();

    public MainPage() {
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing() {
        base.OnAppearing();
        await this.DoSafe(async() => {
            viewModel.AllDocuments = await Repository.GetDocuments();
            if (viewModel.AllDocuments == null)
                return;
            if (viewModel.AllDocuments.Count <= 5) {
                viewModel.TopDocuments = viewModel.AllDocuments;
                return;
            }

            viewModel.TopDocuments = viewModel.AllDocuments.OrderByDescending(x => x.Likes).Take(5).ToList();
        });
    }

    private async void Button_Clicked(object sender, EventArgs e) {
        await Navigation.PushAsync(comixMakerPage);
    }

    private async void Button_Clicked2(object sender, EventArgs e) {
        await Navigation.PushAsync(profilePage);
    }

    private async void CollectionView_SelectionChanged(object sender, CollectionViewSelectionChangedEventArgs e) {
        var item = e.AddedItems.FirstOrDefault() as Document;
        if (item == null)
            return;

        await Navigation.PushAsync(new PdfViewerPage(item.Url));
        ((DXCollectionView)sender).SelectedItem = null;
    }

    private async void Like_Clicked(object sender, EventArgs e) {
        if (sender is not DXButton button)
            return;
        var item = button.BindingContext as Document;
        if (item == null || item.Id == null)
            return;

        button.IsEnabled = false;
        await this.DoSafe(async() => {
            var response = await Repository.LikeDocument(item.Id);
            item.Likes = response?.Likes ?? item.Likes;
            item.IsNotLiked = false;
            button.Content = response?.Likes.ToString();
        });
    }
}

