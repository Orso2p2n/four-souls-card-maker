using Godot;
using Godot.Collections;
using System;

public partial class ScaleBox : NinePatchRect
{
	public MoveableArt parentArt;

	[Export] Control topLeftCorner;
	[Export] Control botLeftCorner;
	[Export] Control topRightCorner;
	[Export] Control botRightCorner;
	[Export] Control rotPoint;

	bool holdingCorner = false;
	bool holdingRotPoint = false;

	Vector2 baseMousePos = Vector2.Zero;

	public override void _Ready() {
		base._Ready();
		
		Card.instance.InputGrabbed += OnInputGrabbed;
	}

	void OnInputGrabbed(InputEvent @event) {
		if (@event is InputEventMouseButton mouseButtonEvent) {
			// Release to unselect
			if (mouseButtonEvent.ButtonIndex == MouseButton.Left) {
				if (!mouseButtonEvent.Pressed) {
					holdingCorner = false;
					holdingRotPoint = false;
				}
			}
		}

		// Move to move
		if (@event is InputEventMouseMotion mouseMotionEvent) {
			if (holdingCorner) {
				OnCornerMoved(mouseMotionEvent.Position);
			}
			else if (holdingRotPoint) {
				OnRotPointMoved(mouseMotionEvent.Position);
			}
		}
    }

	void OnCornerClicked() {
		holdingCorner = true;
	}

	void OnRotationPointClicked() {
		holdingRotPoint = true;
	}

	void OnCornerMoved(Vector2 mousePos) {
		var parentHalfSize = parentArt.Size / 2;
		var baseDist = parentHalfSize.Length();

		var parentCenter = parentArt.Position + parentHalfSize;
		var newPosToCenter = mousePos - parentCenter;
		var newDist = newPosToCenter.Length();

		var newScale = newDist / baseDist;

		parentArt.SetScale(newScale);
	}

	void OnRotPointMoved(Vector2 mousePos) {
		var parentHalfSize = parentArt.Size / 2;
		var parentCenter = parentArt.Position + parentHalfSize;

		var angleToMousePos = parentCenter.AngleToPoint(mousePos) - Mathf.Pi/2;

		parentArt.Rotation = angleToMousePos;
	}
}
