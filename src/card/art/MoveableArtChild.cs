using Godot;
using System;

public partial class MoveableArtChild : TextureRect
{
	public MoveableArt parentArt;

	bool selected;

	public void SetTexture(Texture2D texture) {
		Texture = texture;
		
		PivotOffset = Size / 2;

		if (parentArt.Texture == null) {
			parentArt.Size = Size;
			parentArt.PivotOffset = PivotOffset;
		}
	}
}
