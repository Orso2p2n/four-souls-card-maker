using Godot;
using Godot.Collections;
using System;

public partial class SubTypeMenu : TypeMenu
{
	public void SetList(Array<CardType> newList) {
		cardTypes = newList;

		UpdateItems();

		OnItemSelected(0);
	}
}
