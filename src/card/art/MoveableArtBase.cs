using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class MoveableArtBase : TextureRect
{
	[Signal] public delegate void TextureSetEventHandler();
	
	public string customId;

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

		EmitSignal(SignalName.TextureSet);
	}

	public virtual Dictionary Save() {
		var dict = new Dictionary();

		dict.Add("CustomId", customId);
		dict.Add("UsesCustomPath", usesCustomPath);
		if (usesCustomPath) {
			dict.Add("CustomPath", customPath);
		}

		return dict;
	}

	public async virtual Task Load(Dictionary data) {
        var loadedCustomId = (string) data["CustomId"];
		customId = loadedCustomId;

		var loadedUsesCustomPath = (bool) data["UsesCustomPath"];
		if (loadedUsesCustomPath) {
			usesCustomPath = true;
			var path = (string) data["CustomPath"];
			var texture = EditManager.instance.LoadTextureFromPath(path);
			SetTexture(texture, path);
			await ToSignal(this, "TextureSet");
		}
		else {
			await ToSignal(RenderingServer.Singleton, "frame_post_draw");
		}

    }
}
