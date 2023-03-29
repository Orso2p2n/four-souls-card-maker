using Godot;
using System;

public partial class SetValueButton : SelectionProp
{
	[Export] LineEdit lineEdit;

	public override void _Ready() {
		base._Ready();
	}

	public override void HandleArtProperties(MoveableArt art) {
		SetEnabled(art.canSetValue);
		lineEdit.Text = art.canSetValue ? art.value : null;
	}

	void OnTextChanged(string text) {
		Card.instance.curSelectedArt.value = text;
	}
}
