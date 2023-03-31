using Godot;
using System;

public partial class DescContainer : VBoxContainer
{
	Vector2 basePos;
	Vector2 baseSize;

	public override void _Ready() {
		basePos = Position;
		baseSize = Size;
	}

	public void SetOffsets(float topOffset, float botOffset) {
		Position = basePos + new Vector2(0, topOffset);
		Size = baseSize - new Vector2(0, topOffset + botOffset);
	}
}
