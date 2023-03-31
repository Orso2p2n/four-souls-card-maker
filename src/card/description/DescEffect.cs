using Godot;
using System;

[Tool]
public partial class DescEffect : Control
{
    [Export] public RichTextLabel richText;
    [Export] public int padding;

    public override void _Process(double delta) {
        base._Process(delta);

        if (richText == null) {
            return;
        }

        var height = richText.Size.Y;

        CustomMinimumSize = Size = new Vector2(0, height + padding * 2);

        richText.Position = new Vector2(0, padding);

        richText.Size = new Vector2(richText.Size.X, 0);
    }

    public void SetText(string text) {
        richText.Text = "[center]" + text;
    }
}
