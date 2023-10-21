using DevExpress.Maui.CollectionView;
using Origami.Api;

namespace Origami.ComixMaker;

public partial class ProfilePage : ContentPage {
    public ProfilePage() {
        InitializeComponent();
    }

    protected override async void OnAppearing() {
        base.OnAppearing();
        await this.DoSafe(async() => {
            var response = await Repository.GetDocuments();
            if (response == null)
                return;
            if (response.Count <= 2) {
                BindingContext = response;
                return;
            }

            BindingContext = response.Take(2).ToList();
        });
    }

    private async void CollectionView_SelectionChanged(object sender, CollectionViewSelectionChangedEventArgs e) {
        var item = e.AddedItems.FirstOrDefault() as Document;
        if (item == null)
            return;

        await Navigation.PushAsync(new PdfViewerPage(item.Url));
        ((DXCollectionView)sender).SelectedItem = null;
    }
}

