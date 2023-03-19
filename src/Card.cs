using Godot;
using Godot.Collections;
using System;

[Tool]
public partial class Card : Control
{
	public static Card instance;

	public override void _Ready() {
		instance = this;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
