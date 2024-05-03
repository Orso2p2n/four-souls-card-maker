using Godot;
using System;

public partial class TrashProps : SelectionProp
{
	public override void _Ready() {
		base._Ready();
	}

	public override void HandleArtProperties(MoveableArt art) {
		SetEnabled(art.canBeTrashed);
	}

	public void OnPressed() {
		Card.instance.curSelectedArt.TryTrash(true);
	}
}
