using Godot;
using Godot.Collections;
using System;

public partial class MoveableArtBase : TextureRect
{
	public bool usesCustomPath;
	public string customPath;

	public async void SetTexture(Texture2D texture, string path = "") {
		customPath = path;
		usesCustomPath = (path != "");

		Texture = texture;
		
		await ToSignal(RenderingServer.Singleton, "frame_post_draw");

		PostSetTexture();
	}
	
	public virtual void PostSetTexture() {		
		var halfSize = Size / 2;
		PivotOffset = halfSize;
	}

	public virtual Dictionary Save() {
		var dict = new Dictionary();

		dict.Add("UsesCustomPath", usesCustomPath);
		if (usesCustomPath) {
			dict.Add("CustomPath", customPath);
		}

		return dict;
	}

	public virtual void Load(Dictionary data) {
		var usesCustomPath = (bool) data["UsesCustomPath"];
		if (usesCustomPath) {
			var path = (string) data["CustomPath"];
			var texture = EditManager.instance.LoadTextureFromPath(path);
			SetTexture(texture, path);
		}
	}
}
