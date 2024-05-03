using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class MoveableArt : MoveableArtBase
{
	// Exports
	[Export] MoveableArtChild childArt;
	[Export] public float minScale = 0.1f;
	[Export] public float maxScale = 1.5f;
	[Export] public float scaleStep = 0.01f;
	[Export] public Vector2 baseMinPos = new Vector2(0f, 0f);
	[Export] public Vector2 baseMaxPos = new Vector2(962f, 1312f);
	[Export] public float rotationStep = 1f;
	[Export] public bool canResetScale;
	[Export] public bool canResetPosition;
	[Export] public bool canResetRotation;
	[Export] public bool canSetValue;
	[Export] public bool canBeTrashed;

	// Signals
	[Signal] public delegate void PositionChangedEventHandler(Vector2 pos);
	[Signal] public delegate void ScaleChangedEventHandler(float scale);
	[Signal] public delegate void RotationChangedEventHandler(float scale);

	public string value;

	public Vector2 basePos;
	public Vector2 minPos;
	public Vector2 maxPos;

	bool selected;
	bool mouseIsDown;

	Vector2 movementOffset = Vector2.Zero;

	ScaleBox scaleBox;

	Area2D area2D;
	CollisionShape2D collisionShape2D;
	RectangleShape2D rectangleShape2D;

	bool mouseIsInArea;

	public override void _Ready() {
		Card.instance.InputGrabbed += OnInputGrabbed;

		if (childArt != null) {
			childArt.parentArt = this;
		}

		area2D = new Area2D();
		area2D.ChangeOwner(this);
		collisionShape2D = new CollisionShape2D();
		collisionShape2D.ChangeOwner(area2D);
        rectangleShape2D = new RectangleShape2D {
            Size = Size
        };
        collisionShape2D.Shape = rectangleShape2D;
		area2D.Position = Size/2;
		area2D.InputEvent += OnAreaInputEvent;
		area2D.MouseEntered += OnAreaMouseEntered;
		area2D.MouseExited += OnAreaMouseExited;

		basePos = Position;

		var scaleBoxScene = GD.Load<PackedScene>("res://scenes/card/moveable/scale_box.tscn");
		scaleBox = scaleBoxScene.Instantiate() as ScaleBox;
		scaleBox.ChangeOwner(this);
		scaleBox.parentArt = this;
		scaleBox.Visible = false;

		if (Texture != null) {
			PostSetTexture();
		}
	}

	void OnAreaInputEvent(Node viewport, InputEvent @event, long shapeIdx) {
		// Left click
		if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.ButtonIndex == MouseButton.Left && mouseButtonEvent.Pressed) {
			mouseIsDown = true;
			
			Select();

			if (selected) {
				movementOffset = mouseButtonEvent.Position - Position;
			}

			GetViewport().SetInputAsHandled();
		}
	}

    void OnInputGrabbed(InputEvent @event) {	
		// Left click
		if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.ButtonIndex == MouseButton.Left) {
			mouseIsDown = mouseButtonEvent.Pressed;

			if (mouseIsDown && !mouseIsInArea) {
				Deselect();
			}
		}

		// Mouse movement
		if (@event is InputEventMouseMotion mouseMotionEvent) {
			if (mouseIsDown && selected) {
				SetPosition(mouseMotionEvent.Position - movementOffset);
			}
		}
    }

	void OnAreaMouseEntered() {
		mouseIsInArea = true;
	}

	void OnAreaMouseExited() {
		mouseIsInArea = false;
	}

	public override void _Process(double delta) {
		if (childArt != null) {
			childArt.Position = Position;
			childArt.Scale = Scale;
		}

		if (rectangleShape2D.Size != Size) {
			rectangleShape2D.Size = Size;
		}

		if (area2D.Position != Size/2) {
			area2D.Position = Size/2;
		}

		if (selected) {
			if (scaleBox != null) {
				var scaledSize = Size * Scale + new Vector2(24, 24);
				scaleBox.Position = Position + (Size * ((1-Scale.X)/2)) - new Vector2(12,12);
				scaleBox.Size = scaledSize;
				scaleBox.PivotOffset = scaledSize / 2;
				scaleBox.Rotation = Rotation;
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

	public void SetRotationDegrees(float rotation) {
		if (rotation == RotationDegrees) {
			return;
		}

		var rem = rotation % rotationStep;
		var result = rotation - rem;
		if (rem > rotationStep / 2) {
			result += rotationStep;
		}

		RotationDegrees = result;

		EmitSignal(SignalName.RotationChanged, result);

		SaveManager.instance.OnNeedSaveAction();
	}

	public void TryTrash(bool trashChild = false) {
		if (!canBeTrashed) {
			return;
		}

		Deselect();
		Card.instance.RemoveMoveableArt(this);
		trashCallable.Call(this);

		if (trashChild && childArt != null) {
			childArt.trashCallable.Call(childArt);
		}
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

		if (canResetRotation) {
			dict.Add("Rotation", RotationDegrees);
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

		if (canResetRotation) {
			var rotation = (float) data["Rotation"];
			SetRotationDegrees(rotation);
		}

		if (canSetValue) {
			var val = (string) data["Value"];
			value = val;
		}
	}
}
