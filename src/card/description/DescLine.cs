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

    public void SetState(bool light) {
        Modulate = new Color(1.0f, 1.0f, 1.0f, light ? 0.5f : 1.0f);
    }
}
