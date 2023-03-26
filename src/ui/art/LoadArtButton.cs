using Godot;
using System;

public partial class LoadArtButton : Button
{
	[Export] bool top;
	[Export] TrashArtButton trashButton;
	[Export] ViewArtButton viewButton;

	public bool active;

	public void OnPressed() {
		EditManager.instance.LoadTextureFileDialog(new Callable(this, "SetCardArt"));
	}

	void SetActive(bool active) {
		this.active = active;

		trashButton.Disabled = !active;
		viewButton.Disabled = !active;
	}

	public void Trash() {
		SetActive(false);

		Card.instance.RemoveArt(top);
	}

	public void SetVisible(bool visible) {
		Card.instance.SetArtVisible(visible, top);
	}

	void SetCardArt(Texture2D texture) {
		SetActive(true);
		Card.instance.SetArt(texture, top);
	}
}
