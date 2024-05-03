using Godot;
using System;

public partial class SoulIconMenu : IconMenu
{
	public override void _Ready() {
		linkedArt = Card.instance.soulIcon;

		base._Ready();

		UpdateItems();

		customTextureCallback = new Callable(this, "SetCustomSoulIcon");
	}

	public override void OnItemSelected(long index) {
		base.OnItemSelected(index);

		if (index == customId) {
			return;
		}

		var selectedIcon = cardTypes[(int) index] as SoulIcon;

		Card.instance.SetSoulIcon(selectedIcon);
	}

	public void SetCustomSoulIcon(string path, Texture2D texture) {
		customTexturePath = path;
		Card.instance.SetCustomSoulIcon(texture);
	}
}
