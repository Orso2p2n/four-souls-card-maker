using Godot;
using System;

public partial class MoveableArt : TextureRect
{
	[Export] MoveableArtChild childArt;

	bool selected;
	bool mouseIsDown;

	Vector2 movementOffset = Vector2.Zero;

	public override void _Ready() {
		if (childArt != null) {
			childArt.parentArt = this;
		}
	}

	public override void _Process(double delta) {
		if (selected && childArt != null) {
			childArt.Position = Position;
			childArt.Scale = Scale;
		}
	}

	public void SetTexture(Texture2D texture) {
		Texture = texture;

		PivotOffset = Size / 2;
	}

	public void Select() {
		if (selected) return;

		selected = true;
		GD.Print("Selected " + Name);
	}

	public void Deselect() {
		if (!selected) return;

		selected = false;
		GD.Print("Deselected " + Name);
	}

    public override void _UnhandledInput(InputEvent @event) {
        base._UnhandledInput(@event);
		
		if (@event is InputEventMouseButton mouseButtonEvent) {
			// Click to select
			if (mouseButtonEvent.ButtonIndex == MouseButton.Left) {
				if (mouseButtonEvent.Pressed) {
					mouseIsDown = true;

					var rect = GetRect();
					if (rect.HasPoint(mouseButtonEvent.Position)) {
						movementOffset = mouseButtonEvent.Position - Position;
						Select();
						GetViewport().SetInputAsHandled();
					}
					else {
						Deselect();
					}
				}
				else {
					mouseIsDown = false;
				}

			}
		}
		// Move to move
		else if (@event is InputEventMouseMotion mouseMotionEvent) {
			if (mouseIsDown && selected) {
				Position = mouseMotionEvent.Position - movementOffset; 
			}
		}
    }
}
