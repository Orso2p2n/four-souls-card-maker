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

	bool cornerHeld;

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
						cornerHeld = true;

						GD.Print("Got a corner");

						GetViewport().SetInputAsHandled();
					}
				}
				else {
					cornerHeld = false;
				}

			}
		}
		// Move to move
		else if (@event is InputEventMouseMotion mouseMotionEvent) {
			if (cornerHeld) {
				OnCornerMoved(mouseMotionEvent.Position);
			}
		}
    }

	void OnCornerMoved(Vector2 newPos) {
		var parentHalfSize = parentArt.Size / 2;
		var baseDist = parentHalfSize.Length();

		var parentCenter = parentArt.Position + parentHalfSize;
		var newPosToCenter = newPos - parentCenter;
		var newDist = newPosToCenter.Length();

		var newScale = newDist / baseDist;

		newScale = Mathf.Clamp(newScale, 0.1f, 1.5f);

		parentArt.Scale = new Vector2(newScale, newScale);
	}
}
