using Godot;
using System;

public partial class TrashArtButton : Button
{
	[Export] LoadArtButton mainButton;

	void OnPressed() {
		mainButton.Trash();
	}
}
