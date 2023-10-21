
namespace Origami.ComixMaker;

public class ComixFrameBuilder {
    public string BackgroundImagePath { get; set; }
    public string PersonImagePath { get; set; }
    public string Text { get; set; }

    public ComixFrameBuilder() {
        BackgroundImagePath = string.Empty;
        PersonImagePath = string.Empty;
        Text = string.Empty;
    }
}