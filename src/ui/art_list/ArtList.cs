using Godot;
using Godot.Collections;
using System;

public partial class ArtList : ItemList
{
	[Export] Dictionary<string,PackedScene> scenes = new Dictionary<string,PackedScene>{
		{"Tap tag", new PackedScene()},
		{"Paid tag", new PackedScene()},
		{"Level tag", new PackedScene()},
		{"Custom", new PackedScene()}
	};

	Array<MoveableArt> arts = new Array<MoveableArt>();

	MoveableArt curEditingCustomArt;

	public MoveableArt AddItem(string id) {
		if (!scenes.ContainsKey(id)) {
			return null;
		}

		var scene = scenes[id];
		var instance = scene.Instantiate();
		var moveableArt = instance as MoveableArt;
		moveableArt.trashCallable = new Callable(this, "RemoveEntryFromArt");

		if (id == "Custom") {
			SetCustomProps(moveableArt);
		}
		else {
			AddEntryFromArt(moveableArt);
		}

		Card.instance.AddMoveableArt(moveableArt);

		return moveableArt;
	}

	void SetCustomProps(MoveableArt art) {
		curEditingCustomArt = art;
		EditManager.instance.LoadTextureFileDialog(new Callable(this, "SetCustomArt"));
	}

	void SetCustomArt(string path, Texture2D texture) {
		curEditingCustomArt.SetTexture(texture, path);
		AddEntryFromArt(curEditingCustomArt);
		curEditingCustomArt = null;
	}

	void AddEntryFromArt(MoveableArt art) {
		AddIconItem(art.Texture);
		arts.Add(art);
	}

	void RemoveEntryFromArt(MoveableArt art) {
		var index = arts.IndexOf(art);
		arts.RemoveAt(index);
		RemoveItem(index);

		art.GetParent().RemoveChild(art);
		art.Dispose();
	}

	void OnItemSelected(int index) {
		var art = arts[index];
		art.Select();
	}

	public void OnEmptyClicked(Vector2 pos, int mouseButtonIndex) {
		DeselectAll();
		foreach (var art in arts) {
			art.Deselect();
		}
	}
}
