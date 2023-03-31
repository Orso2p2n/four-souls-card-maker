using Godot;
using Godot.Collections;
using System;

public partial class MainTypeMenu : TypeMenu
{
	[Export] BackgroundMenu backgroundMenu;
	[Export] ForegroundMenu foregroundMenu;
	[Export] BorderMenu 	borderMenu;

	public override void _Ready() {
		base._Ready();

		UpdateItems();
	}

	public override void OnItemSelected(long index) {
		backgroundMenu.Clear();
		backgroundMenu.Clear();
		borderMenu.Clear();

		var selectedCardType = cardTypes[(int) index] as CardMainType;

		var bgList = new Array<MenuItem>();
		foreach (var subType in selectedCardType.backgrounds) {
			bgList.Add(subType);
		}
		backgroundMenu.SetList(bgList);

		var fgList = new Array<MenuItem>();
		foreach (var subType in selectedCardType.foregrounds) {
			fgList.Add(subType);
		}
		foregroundMenu.SetList(fgList);

		var borderList = new Array<MenuItem>();
		foreach (var subType in selectedCardType.borders) {
			borderList.Add(subType);
		}
		borderMenu.SetList(borderList);

		EditManager.instance.SetStats(selectedCardType.stats);

		Card.instance.SetDescOffsets(selectedCardType.descOffsetTop, selectedCardType.descOffsetBot);
	}
}
