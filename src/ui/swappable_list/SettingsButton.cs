using Godot;
using System;

public partial class SettingsButton : Button
{
	[Export] Control popupMenu;

	public void OnPressed() {
		ToggleMenu();

		var pos = GlobalPosition;
		pos += new Vector2(-popupMenu.Size.X / 2 + Size.X/2, Size.Y);
		popupMenu.Position = pos;
	}

	void ToggleMenu() {
		popupMenu.Visible = !popupMenu.Visible;
	}

    public override void _Input(InputEvent @event) {
        base._Input(@event);

		if (@event is InputEventMouseButton mouseButtonEvent && mouseButtonEvent.ButtonIndex == MouseButton.Left && mouseButtonEvent.Pressed) {
			if (!popupMenu.Visible) {
				return;
			}

			var point = mouseButtonEvent.GlobalPosition;
			var thisRect = GetGlobalRect();
			var popupMenuRect = popupMenu.GetGlobalRect();

			if (!thisRect.HasPoint(point) && !popupMenuRect.HasPoint(point)) {
				ToggleMenu();
			}
		}
    }
}
