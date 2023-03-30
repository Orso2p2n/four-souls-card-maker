using Godot;
using System;

public partial class SoulIconMenu : TypeMenu
{
	public override void _Ready() {
		base._Ready();

		UpdateItems();
	}

	public override void OnItemSelected(long index) {
		base.OnItemSelected(index);

		var selectedIcon = cardTypes[(int) index] as SoulIcon;

		Card.instance.SetSoulIcon(selectedIcon);
	}
}
