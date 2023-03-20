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

        GD.Print("tes");

        var height = richText.Size.Y;

        CustomMinimumSize = new Vector2(0, height + padding * 2);

        richText.SetPosition(new Vector2(0, padding));
    }

    public void SetText(string text) {
        richText.Text = "[center]" + text;
    }
}
