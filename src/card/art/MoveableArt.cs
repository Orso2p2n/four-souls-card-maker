using Godot;
using System;

public partial class MoveableArt : TextureRect
{
	[Export] MoveableArtChild childArt;
	[Export] ScaleBox scaleBox;
	[Export] public bool canResetScale;
	[Export] public bool canResetPosition;
	[Export] public bool canSetValue;

	public string value;

	bool selected;
	bool mouseIsDown;

	Vector2 movementOffset = Vector2.Zero;

	public override void _Ready() {
		if (childArt != null) {
			childArt.parentArt = this;
		}

		if (scaleBox != null) {
			scaleBox.parentArt = this;
			scaleBox.Visible = false;
		}
	}

	public override void _Process(double delta) {
		if (selected && childArt != null) {
			if (childArt != null) {
				childArt.Position = Position;
				childArt.Scale = Scale;
			}

			if (scaleBox != null) {
				var scaledSize = Size * Scale;
				scaleBox.Position = Position + (Size * ((1-Scale.X)/2));
				scaleBox.Size = scaledSize;
				scaleBox.PivotOffset = scaledSize / 2;
			}
		}
	}

	public void SetTexture(Texture2D texture) {
		Texture = texture;

		PivotOffset = Size / 2;
	}

	public void Select() {
		if (selected) return;

		selected = true;

		scaleBox.Visible = true;

		Card.instance.OnSelectedArt(this);
	}

	public void Deselect() {
		if (!selected) return;

		selected = false;

		scaleBox.Visible = false;

		Card.instance.OnDeselectedArt(this);
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
