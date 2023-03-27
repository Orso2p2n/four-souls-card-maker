using Godot;
using System;

public partial class CardPreview : SubViewportContainer
{
	[Export] SubViewport viewport;

    public override void _Ready() {
        base._Ready();

		viewport.HandleInputLocally = true;
    }
}
