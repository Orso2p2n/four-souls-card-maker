using Godot;
using System;

[Tool]
public partial class DescLine : Control
{
    [Export] public int padding;

    public void SetState(bool light) {
        Modulate = new Color(1.0f, 1.0f, 1.0f, light ? 0.5f : 1.0f);
    } 
}
