using Godot;
using System;

[Tool]
public partial class DescLine : Control
{
    [Export] public int padding;

    Vector2 baseSize;

    public override void _Ready() {
        base._Ready();

        baseSize = Size;
    }

    public override void _Process(double delta) {
        base._Process(delta);

        Size = baseSize + new Vector2(0, padding);
    }

    public void SetAlpha(float alpha) {
        Visible = true;
        Modulate = new Color(1.0f, 1.0f, 1.0f, alpha);
    }
}
