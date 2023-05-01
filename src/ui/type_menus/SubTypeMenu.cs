using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class SubTypeMenu : TypeMenu
{
	[Export] public Texture2D customIcon;
	public int customId = -1;

	public string customTexturePath;

	public Callable customTextureCallback;

	public void SetList(Array<MenuItem> newList) {
		cardTypes = newList;

		UpdateItems();
	}

	public override void UpdateItems() {
		base.UpdateItems();

		AddCustomEntry();

		OnItemSelected(0);
	}

	void AddCustomEntry() {
		customId = ItemCount;
		AddItem("Custom...", customId);
		SetItemIcon(customId, customIcon);
	}

	public override void OnItemSelected(long index) {
		base.OnItemSelected(index);

		if (index == customId) {
			EditManager.instance.LoadTextureFileDialog(customTextureCallback);
		}
	}

	public void ResetSelection() {
		Selected = 0;
		OnItemSelected(0);
	}

	// --- SAVE HANDLING ---
	public override Dictionary Save() {
		var dict = base.Save();

		if (Selected == customId) {
			dict.Add("CustomPath", customTexturePath);
		}

		return dict;
	}

	public override Task Load(Dictionary data) {
        base.Load(data);

		if (Selected == customId) {
			var path = (string) data["CustomPath"];
			var texture = EditManager.instance.LoadTextureFromPath(path);
			customTextureCallback.Call(path, texture);
		}

		return Task.CompletedTask;
	}
}
