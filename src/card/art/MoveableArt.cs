using Godot;
using System;

public partial class MoveableArt : TextureRect
{
	// Exports
	[Export] MoveableArtChild childArt;
	[Export] ScaleBox scaleBox;
	[Export] public float minScale = 0.1f;
	[Export] public float maxScale = 1.5f;
	[Export] public Vector2 minPos = new Vector2(-962f, -1312f);
	[Export] public Vector2 maxPos = new Vector2(962f, 1312f);
	[Export] public bool canResetScale;
	[Export] public bool canResetPosition;
	[Export] public bool canSetValue;

	// Signals
	[Signal] public delegate void PositionChangedEventHandler(Vector2 pos);
	[Signal] public delegate void ScaleChangedEventHandler(float scale);

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
				SetPosition(mouseMotionEvent.Position - movementOffset);
			}
		}
    }

	public void SetX(float x) {
		SetPosition(new Vector2(x, Position.Y));
	}

	public void SetY(float y) {
		SetPosition(new Vector2(Position.X, y));
	}

	public void SetPosition(Vector2 pos) {
		if (pos == Position) {
			return;
		}

		pos = pos.Clamp(minPos, maxPos);
		Position = pos;

		EmitSignal(SignalName.PositionChanged, pos);
	}

	public void SetScale(float scale) {
		if (scale == Scale.X) {
			return;
		}

		scale = Mathf.Clamp(scale, minScale, maxScale);
		Scale = new Vector2(scale, scale);

		EmitSignal(SignalName.ScaleChanged, scale);
	}
}
