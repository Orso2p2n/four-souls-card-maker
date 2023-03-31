using Godot;
using System;

public partial class MoveableArtChild : MoveableArtBase
{
	public MoveableArt parentArt;

	bool selected;

	public override void PostSetTexture() {	
		base.PostSetTexture();

		if (parentArt.Texture == null) {
			parentArt.Size = Size;
			parentArt.PivotOffset = PivotOffset;
		}
	}
}
