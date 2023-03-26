using Godot;
using Godot.Collections;
using System;

public partial class SubTypeMenu : TypeMenu
{
	[Export] public Texture2D customIcon;
	public int customId = -1;

	public void SetList(Array<CardType> newList) {
		cardTypes = newList;

		UpdateItems();
		AddCustomEntry();

		OnItemSelected(0);
	}

	void AddCustomEntry() {
		var customId = ItemCount;
		AddItem("Custom...", customId);
		SetItemIcon(customId, customIcon);
	}

	public override void OnItemSelected(long index) {
		base.OnItemSelected(index);

		if (index == customId) {
			// do stuff
		}
	}
}
