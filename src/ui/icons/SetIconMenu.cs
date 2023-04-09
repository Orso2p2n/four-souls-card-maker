using Godot;
using System;

public partial class SetIconMenu : IconMenu
{
	public override void _Ready() {
		base._Ready();

		linkedArt = Card.instance.setIcon;
		linkedArt.trashCallable = new Callable(this, "ResetSelection");
	
		UpdateItems();

		customTextureCallback = new Callable(this, "SetCustomSetIcon");
	}

	public override void OnItemSelected(long index) {
		base.OnItemSelected(index);

		if (index == customId) {
			return;
		}

		var selectedIcon = cardTypes[(int) index] as SetIcon;

		Card.instance.SetSetIcon(selectedIcon);
	}

	public void SetCustomSetIcon(string path, Texture2D texture) {
		customTexturePath = path;
		Card.instance.SetCustomSetIcon(texture);
	}
}
