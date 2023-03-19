using Godot;
using System;

public partial class SwappableSeparator : SwappableItem
{
    public OptionButton lineSelection;

    public override void _Ready() {
        base._Ready();

        lineSelection = content as OptionButton;
    }
}
