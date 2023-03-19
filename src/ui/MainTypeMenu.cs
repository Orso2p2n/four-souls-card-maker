using Godot;
using Godot.Collections;
using System;

public partial class MainTypeMenu : TypeMenu
{
	[Export] SubTypeMenu subTypeMenu;

    public override void _Ready() {
        base._Ready();

		UpdateItems();
    }

	public override void OnItemSelected(long index) {
		subTypeMenu.Clear();

		var selectedCardType = cardTypes[(int) index] as CardMainType;

		var newList = new Array<CardType>();
		foreach (var subType in selectedCardType.subTypes) {
			newList.Add(subType);
		}

		subTypeMenu.SetList(newList);
	}
}
