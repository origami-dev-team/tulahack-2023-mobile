using System.ComponentModel;
using Origami.Api;

namespace Origami.ComixMaker;

public class MainViewModel : INotifyPropertyChanged {
    public event PropertyChangedEventHandler? PropertyChanged;


    private List<Document>? allDocuments;
    public List<Document>? AllDocuments {
        get => allDocuments;
        set {
            allDocuments = value;
            OnPropertyChanged(nameof(AllDocuments));
        }
    }

    private List<Document>? topDocuments;
    public List<Document>? TopDocuments {
        get => topDocuments;
        set {
            topDocuments = value;
            OnPropertyChanged(nameof(TopDocuments));
        }
    }

    private void OnPropertyChanged(string propertyName) {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}