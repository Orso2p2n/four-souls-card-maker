using Godot;
using System;

public partial class ScaleBoxCorner : Control
{
	[Signal] public delegate void ClickedEventHandler();

	Area2D area2D;
	CollisionShape2D collisionShape2D;
	RectangleShape2D rectangleShape2D;

	public override void _Ready() {
		area2D = new Area2D();
		area2D.ChangeOwner(this);
		area2D.Position = Size/2;
		area2D.Priority = -20;

		collisionShape2D = new CollisionShape2D();
		collisionShape2D.ChangeOwner(area2D);

        rectangleShape2D = new RectangleShape2D {
            Size = Size
        };

        collisionShape2D.Shape = rectangleShape2D;

		area2D.InputEvent += OnArea2DInputEvent;
	}

	void OnArea2DInputEvent(Node viewport, InputEvent @event, long shapeIdx) {
		if (@event is InputEventMouseButton mouseButtonEvent) {
			if (mouseButtonEvent.ButtonIndex == MouseButton.Left) {
				if (mouseButtonEvent.Pressed) {
					EmitSignal(SignalName.Clicked);
					GetViewport().SetInputAsHandled();
				}
			}
		}
	}
}
