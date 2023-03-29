using Godot;
using System;

public partial class NewDescriptionItemButton : MenuButton
{
	[Export] public SwappableList list;
	PopupMenu popupMenu;

	public override void _Ready() {
		base._Ready();

		popupMenu = GetPopup();
		popupMenu.IndexPressed += OnItemPressed;
	}

	public void OnItemPressed(long index) {
		var id = (int) index;
		var text = popupMenu.GetItemText(id);
		list.AddItem(text);
	}
}
