using Godot;
using Godot.Collections;
using System;

public partial class ArtList : ItemList
{
	[Export] Dictionary<string,PackedScene> scenes = new Dictionary<string,PackedScene>{
		{"Tap tag", new PackedScene()},
		{"Paid tag", new PackedScene()},
		{"Level tag", new PackedScene()},
		{"Custom...", new PackedScene()}
	};

	Array<MoveableArt> arts = new Array<MoveableArt>();

	MoveableArt curEditingCustomArt;

	public override void _Ready() {
		Card.instance.SelectedArtChanged += OnSelectedArtChanged;
	}

	void OnSelectedArtChanged(MoveableArt moveableArt) {
		var i = 0;
		foreach (var art in arts) {
			if (art == moveableArt) {
				Select(i);
				return;
			}

			i++;
		}

		DeselectAll();
	}

	public MoveableArt AddItem(string id, bool addEntry = true) {
		if (!scenes.ContainsKey(id)) {
			return null;
		}

		var scene = scenes[id];
		var instance = scene.Instantiate();
		var moveableArt = instance as MoveableArt;
		moveableArt.customId = id;
		moveableArt.trashCallable = new Callable(this, "RemoveEntryFromArt");

		if (addEntry) {
			if (id == "Custom...") {
				SetCustomProps(moveableArt);
			}
			else {
				AddEntryFromArt(moveableArt);
			}
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
	
    // --- SAVE HANDLING ---
	public Dictionary Save() {
		var dict = new Dictionary();

		var items = new Array<Dictionary>();

		foreach (var art in arts) {
			if (art == null) {
				continue;
			}

			var artDict = art.Save();
			items.Add(artDict);
		}

		dict.Add("Arts", items);

		return dict;
	}

	public async void Load(Dictionary data) {
		foreach (var art in arts.Duplicate()) {
			art.TryTrash();
		}

		var loadedArts = (Array<Dictionary>) data["Arts"];

		foreach (Dictionary loadedArt in loadedArts) {
			var customId = (string) loadedArt["CustomId"];
			var moveableArt = AddItem(customId, false);

			await moveableArt.Load(loadedArt);

			AddEntryFromArt(moveableArt);
		}
	}
}
