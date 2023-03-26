using Godot;
using System;

public partial class ViewArtButton : Button
{
	[Export] LoadArtButton mainButton;

	[Export] Texture2D openIcon;
	[Export] Texture2D closedIcon;

	bool visible = true;

	void OnPressed() {
		SetVisible(!visible);
	}

	public void SetVisible(bool visible) {
		this.visible = visible;

		mainButton.SetVisible(visible);

		Icon = visible ? openIcon : closedIcon;
	}
}
