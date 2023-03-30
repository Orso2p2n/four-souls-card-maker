using Godot;
using Godot.Collections;
using System;

public partial class SubTypeMenu : TypeMenu
{
	[Export] public Texture2D customIcon;
	public int customId = -1;

	public Callable customTextureCallback;

	public void SetList(Array<MenuItem> newList) {
		cardTypes = newList;

		UpdateItems();
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
}
