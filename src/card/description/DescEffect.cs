using Godot;
using System;

[Tool]
public partial class DescEffect : Control
{
    [Export] public RichTextLabel richText;
    [Export] public int padding;

    [Signal] public delegate void SizeChangedEventHandler();

    int baseFontSize;
    int curFontSize;

    public float userScale = 1.0f;
    public float systemScale = 1.0f;

    public override void _Ready() {
        base._Ready();

        baseFontSize = curFontSize = richText.GetThemeFontSize("normal_font_size");

        UpdateSize();
    }

    public override void _Process(double delta) {
        base._Process(delta);
    }

    public void SetText(string text) {
        richText.Text = "[center]" + text;
        UpdateSize();
    }

    public void SetUserScale(float value) {
        userScale = value;
        UpdateSize();
    }

    public void SetSystemScale(float value) {
        systemScale = value;
        UpdateSize();
    }

    public async void UpdateSize(bool emitSignal = true) {
        if (richText == null) {
            return;
        }

        curFontSize = (int) (baseFontSize * userScale * systemScale);
        richText.AddThemeFontSizeOverride("normal_font_size", curFontSize);
        richText.Position = new Vector2(0, padding);
        richText.Size = new Vector2(richText.Size.X, 0);

        await ToSignal(richText, "resized");

        var oldSize = Size;
        var height = richText.Size.Y;

        CustomMinimumSize = Size = new Vector2(0, height + padding);

        if (Size != oldSize && emitSignal) {
            EmitSignal(SignalName.SizeChanged);
        }
    }
}
