using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class MoveableArtBase : TextureRect
{
	[Signal] public delegate void TextureSetEventHandler();

	public Callable trashCallable;
	
	public string customId;

	public bool usesCustomPath;
	public string customPath;

	public async void SetTexture(Texture2D texture, string path = "") {
		customPath = path;
		usesCustomPath = (path != "");

		Texture = texture;
		
		await ToSignal(RenderingServer.Singleton, "frame_post_draw");
		await ToSignal(RenderingServer.Singleton, "frame_post_draw"); // HACK fix because the size was still at (962,0) even after the first frame_post_draw

		PostSetTexture();

		SaveManager.instance.OnNeedSaveAction();
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
			var localPath = SaveManager.instance.GlobalPathToLocal(customPath);
			dict.Add("CustomPath", localPath);
		}

		return dict;
	}

	public async virtual Task Load(Dictionary data) {
        var loadedCustomId = (string) data["CustomId"];
		customId = loadedCustomId;

		var loadedUsesCustomPath = (bool) data["UsesCustomPath"];
		if (loadedUsesCustomPath) {
			usesCustomPath = true;
			var localPath = (string) data["CustomPath"];
			var globalPath = SaveManager.instance.LocalPathToGlobal(localPath);
			var texture = EditManager.instance.fileDialog.LoadTextureFromPath(globalPath);
			SetTexture(texture, globalPath);
			await ToSignal(this, "TextureSet");
		}
		else {
			await ToSignal(RenderingServer.Singleton, "frame_post_draw");
		}

    }
}
