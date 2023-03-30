using Godot;
using Godot.Collections;
using System;

public partial class TypeMenu : OptionButton
{
	[Export] public Array<MenuItem> cardTypes;

	public override void _Ready() {
		base._Ready();

		ItemSelected += OnItemSelected;
	}

	public virtual void UpdateItems() {
		Clear();

		var id = 0;
		foreach (var cardType in cardTypes) {
			AddItem(cardType.name, id);

			if (cardType.icon != null) {
				SetItemIcon(id, cardType.icon);
			}

			id++;
		}

		OnItemSelected(0);
	}

	public virtual void OnItemSelected(long index) {}
}
