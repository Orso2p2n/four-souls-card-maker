using Godot;
using System;

public partial class SetIconMenu : SubTypeMenu
{
	public override void _Ready() {
		base._Ready();

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

	public void SetCustomSetIcon(Texture2D texture) {
		Card.instance.SetCustomSetIcon(texture);
	}
}
