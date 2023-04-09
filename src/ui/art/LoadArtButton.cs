using Godot;
using Godot.Collections;
using System;

public partial class LoadArtButton : Button
{
	[Export] bool top;
	[Export] TrashArtButton trashButton;
	[Export] ViewArtButton viewButton;

	public bool active;
	public string path;
	public MoveableArtBase linkedArt;

	public override void _Ready() {
		base._Ready();

		linkedArt = top ? Card.instance.topArt : Card.instance.art;
	}

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

	void SetCardArt(string path, Texture2D texture) {
		SetActive(true);
		this.path = path;
		Card.instance.SetArt(texture, top);
	}

	// --- SAVE HANDLING ---
	public virtual Dictionary Save() {
		var dict = new Dictionary();

		dict.Add("Active", active);

		if (!active) {
			return dict;
		}
		
		dict.Add("Path", path);

		if (top) {
			return dict;
		}

		dict.Add("X", linkedArt.Position.X);
		dict.Add("Y", linkedArt.Position.Y);
		dict.Add("Scale", linkedArt.Scale.X);

		return dict;
	}

	public async virtual void Load(Dictionary data) {
		SetActive((bool) data["Active"]);

		if (!active) {
			return;
		}

		var path = (string) data["Path"];
		var texture = EditManager.instance.LoadTextureFromPath(path);
		SetCardArt(path, texture);

		if (top) {
			return;
		}

		await ToSignal(RenderingServer.Singleton, "frame_post_draw");

		var linkedMainArt = linkedArt as MoveableArt;

		var x = (float) data["X"];
		var y = (float) data["Y"];
		linkedMainArt.SetPosition(new Vector2(x,y));

		var scale = (float) data["Scale"];
		linkedMainArt.SetScale(scale);
	}
}
