namespace Origami.ComixMaker;

public partial class MainPage : ContentPage {
    private ComixMakerPage comixMakerPage = new ComixMakerPage();
    private ProfilePage profilePage = new ProfilePage();

    public MainPage() {
        InitializeComponent();
    }

    private async void Button_Clicked(object sender, EventArgs e) {
        await Navigation.PushAsync(comixMakerPage);
    }

    private async void Button_Clicked2(object sender, EventArgs e) {
        await Navigation.PushAsync(profilePage);
    }
}

