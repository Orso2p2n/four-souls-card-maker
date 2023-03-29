using Godot;
using System;

public partial class ResetScaleButton : SelectionProp
{
	public override void HandleArtProperties(MoveableArt art) {
		SetEnabled(art.canResetScale);
	}

	void OnPressed() {
		Card.instance.curSelectedArt.Scale = Vector2.One;
	}
}
