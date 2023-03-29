using Godot;
using System;

public partial class ResetPosButton : SelectionProp
{
	public override void HandleArtProperties(MoveableArt art) {
		SetEnabled(art.canResetPosition);
	}

	void OnXPressed() {
		Card.instance.curSelectedArt.Position = new Vector2(0, Card.instance.curSelectedArt.Position.Y);
	}

	void OnYPressed() {
		Card.instance.curSelectedArt.Position = new Vector2(Card.instance.curSelectedArt.Position.X, 0);
	}
}
