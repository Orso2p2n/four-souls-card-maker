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
	Array<Control> corners;

	int heldCornerId = -1;

	Vector2 baseMousePos = Vector2.Zero;

	public override void _Ready() {
		base._Ready();

		corners = new Array<Control>{topLeftCorner, botLeftCorner, topRightCorner, botRightCorner};
	}

	public override void _Process(double delta) {
		base._Process(delta);
	}

	int GetCornerAtPoint(Vector2 point) {
		var i = 0;
		foreach (var corner in corners) {
			var rect = corner.GetGlobalRect();

			if (rect.HasPoint(point)) {
				return i;
			}

			i++;
		}

		return -1;
	}

    public override void _UnhandledInput(InputEvent @event) {
        base._UnhandledInput(@event);
		
		if (@event is InputEventMouseButton mouseButtonEvent) {
			// Click to select
			if (mouseButtonEvent.ButtonIndex == MouseButton.Left) {
				if (mouseButtonEvent.Pressed) {
					var pressedCornerId = GetCornerAtPoint(mouseButtonEvent.Position);

					if (pressedCornerId != -1) {
						var pressedCorner = corners[pressedCornerId];

						baseMousePos = mouseButtonEvent.Position;
						heldCornerId = pressedCornerId;

						GetViewport().SetInputAsHandled();
					}
				}
				else {
					heldCornerId = -1;
				}

			}
		}
		// Move to move
		else if (@event is InputEventMouseMotion mouseMotionEvent) {
			HandleMouseCursor(mouseMotionEvent);

			if (heldCornerId != -1) {
				OnCornerMoved(mouseMotionEvent.Position);
			}
		}
    }

	void HandleMouseCursor(InputEventMouseMotion mouseMotionEvent) {
		var rect = GetRect();
		var mouseIsInside = rect.HasPoint(mouseMotionEvent.Position);

		Control.CursorShape cursorShape = Control.CursorShape.Arrow;

		if (!mouseIsInside && heldCornerId == -1) {
			if (EditManager.instance.GetCursorShape() != cursorShape) {
				EditManager.instance.SetCursorShape(cursorShape);
			}

			return;
		}

		var hoveredCornerId = GetCornerAtPoint(mouseMotionEvent.Position);
		if (hoveredCornerId == -1) {
			if (heldCornerId == -1) {
				return;
			}

			hoveredCornerId = heldCornerId;
		}

		switch (hoveredCornerId) {
			case 0: case 3:
				cursorShape = Control.CursorShape.Fdiagsize;
			break;

			case 1: case 2:
				cursorShape = Control.CursorShape.Bdiagsize;
			break;
		}

		EditManager.instance.SetCursorShape(cursorShape);
	}

	void OnCornerMoved(Vector2 newPos) {
		var parentHalfSize = parentArt.Size / 2;
		var baseDist = parentHalfSize.Length();

		var parentCenter = parentArt.Position + parentHalfSize;
		var newPosToCenter = newPos - parentCenter;
		var newDist = newPosToCenter.Length();

		var newScale = newDist / baseDist;

		parentArt.SetScale(newScale);
	}
}
