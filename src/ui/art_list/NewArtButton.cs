using Godot;
using System;

public partial class NewArtButton : MenuButton
{
	[Export] public ArtList list;
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
