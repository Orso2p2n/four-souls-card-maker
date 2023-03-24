using Godot;
using Godot.Collections;
using System;

public partial class MainTypeMenu : TypeMenu
{
	[Export] BackgroundMenu backgroundMenu;
	[Export] ForegroundMenu foregroundMenu;

	public override void _Ready() {
		base._Ready();

		UpdateItems();
	}

	public override void OnItemSelected(long index) {
		backgroundMenu.Clear();
		foregroundMenu.Clear();

		var selectedCardType = cardTypes[(int) index] as CardMainType;

		var bgList = new Array<CardType>();
		foreach (var subType in selectedCardType.backgrounds) {
			bgList.Add(subType);
		}
		backgroundMenu.SetList(bgList);

		var fgList = new Array<CardType>();
		foreach (var subType in selectedCardType.foregrounds) {
			fgList.Add(subType);
		}
		foregroundMenu.SetList(fgList);
	}
}
