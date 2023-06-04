using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class MoveableArt : MoveableArtBase
{
	// Exports
	[Export] MoveableArtChild childArt;
	[Export] ScaleBox scaleBox;
	[Export] public float minScale = 0.1f;
	[Export] public float maxScale = 1.5f;
	[Export] public float scaleStep = 0.025f;
	[Export] public Vector2 baseMinPos = new Vector2(0f, 0f);
	[Export] public Vector2 baseMaxPos = new Vector2(962f, 1312f);
	[Export] public bool canResetScale;
	[Export] public bool canResetPosition;
	[Export] public bool canSetValue;
	[Export] public bool canBeTrashed;

	// Signals
	[Signal] public delegate void PositionChangedEventHandler(Vector2 pos);
	[Signal] public delegate void ScaleChangedEventHandler(float scale);

	public string value;

	public Vector2 basePos;
	public Vector2 minPos;
	public Vector2 maxPos;

	bool selected;
	bool mouseIsDown;

	Vector2 movementOffset = Vector2.Zero;

	public Callable trashCallable;

	public override void _Ready() {
		if (childArt != null) {
			childArt.parentArt = this;
		}

		if (scaleBox != null) {
			scaleBox.parentArt = this;
			scaleBox.Visible = false;
		}

		basePos = Position;

		if (Texture != null) {
			PostSetTexture();
		}
	}

	public override void _Process(double delta) {
		if (childArt != null) {
			childArt.Position = Position;
			childArt.Scale = Scale;
		}

		if (selected) {
			if (scaleBox != null) {
				var scaledSize = Size * Scale;
				scaleBox.Position = Position + (Size * ((1-Scale.X)/2));
				scaleBox.Size = scaledSize;
				scaleBox.PivotOffset = scaledSize / 2;
			}
		}
	}
	
	public override void PostSetTexture() {	
		var halfSize = GetRect().Size / 2;
		minPos = baseMinPos - halfSize;
		maxPos = baseMaxPos - halfSize;

		base.PostSetTexture();
	}

	public void Select() {
		if (selected || Texture == null) return;

		Card.instance.DeselectAllMoveableArts();

		selected = true;

		scaleBox.Visible = true;

		Card.instance.OnSelectedArt(this);

		return;
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
						Select();

						if (selected) {
							movementOffset = mouseButtonEvent.Position - Position;
							GetViewport().SetInputAsHandled();
						}
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

		SaveManager.instance.OnNeedSaveAction();
	}

	public void SetScale(float scale) {
		if (scale == Scale.X) {
			return;
		}

		var rem = scale % scaleStep;
		var result = scale - rem;
		if (rem > scaleStep / 2) {
			result += scaleStep;
		}

		scale = Mathf.Clamp(result, minScale, maxScale);
		Scale = new Vector2(scale, scale);

		EmitSignal(SignalName.ScaleChanged, scale);

		SaveManager.instance.OnNeedSaveAction();
	}

	public void TryTrash() {
		if (!canBeTrashed) {
			return;
		}

		Deselect();
		Card.instance.RemoveMoveableArt(this);
		trashCallable.Call(this);
	}

	// --- SAVE HANDLING ---
	public override Dictionary Save() {
		var dict = base.Save();

		if (canResetPosition) {
			dict.Add("X", Position.X);
			dict.Add("Y", Position.Y);
		}

		if (canResetScale) {
			dict.Add("Scale", Scale.X);
		}

		if (canSetValue) {
			dict.Add("Value", value);
		}

		return dict;
	}

	public async override Task Load(Dictionary data) {
		await base.Load(data);
		
		if (canResetPosition) {
			var x = (float) data["X"];
			var y = (float) data["Y"];
			SetPosition(new Vector2(x,y));
		}

		if (canResetScale) {
			var scale = (float) data["Scale"];
			SetScale(scale);
		}

		if (canSetValue) {
			var val = (string) data["Value"];
			value = val;
		}
	}
}
