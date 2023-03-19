using Godot;
using Godot.Collections;
using System;

public partial class SwappableList : VBoxContainer
{
	Control placeholder;
	bool isMovingItem;

	[Export] Dictionary<string,PackedScene> scenes = new Dictionary<string,PackedScene>{
		{"Effect", new PackedScene()},
		{"Separator", new PackedScene()},
		{"Lore", new PackedScene()},
		{"Placeholder", new PackedScene()}
	};

	public override void _Ready() {
		base._Ready();
	}

	public Control AddItem(string id, int pos = -1) {
		if (!scenes.ContainsKey(id)) {
			return null;
		}

		var scene = scenes[id];
		var instance = scene.Instantiate();
		var item = instance as Control;
		item.ChangeOwner(this, true);

		if (pos != -1) {
			MoveChild(item, pos);
		}

		if (item is SwappableItem swappableItem) {
			swappableItem.list = this;

			swappableItem.button.GuiInput += (InputEvent @event) => OnGuiInputItem(@event, swappableItem);
		}

		return item;
	}

	public void AddPlaceholder(Control reference, int index) {
		placeholder = AddItem("Placeholder", index);
		placeholder.CustomMinimumSize = new Vector2(placeholder.CustomMinimumSize.X, reference.CustomMinimumSize.Y);
	}

	public void RemovePlaceholder() {
		if (placeholder == null) {
			return;
		}

		RemoveChild(placeholder);
		placeholder.Dispose();
	}

	public void OnGuiInputItem(InputEvent @event, SwappableItem item) {
		if (@event is InputEventMouseMotion eventMouseMotion) {
			OnMouseMoveInItem(eventMouseMotion, item);
		}
	}

	public void OnMouseMoveInItem(InputEventMouseMotion eventMouseMotion, SwappableItem item) {
		if (!isMovingItem) {
			return;
		}

		var centerY = item.Size.Y/2;
		var index = item.GetIndex();

		GD.Print("mouse pos: " + eventMouseMotion.Position.Y + ", centerY: " + centerY + ", index: " + index);

		if (eventMouseMotion.Position.Y > centerY) {
			GD.Print(index + 1);
			MoveChild(item, index + 1);
		}
		else {
			GD.Print(index - 1);
			MoveChild(item, index - 1);
		}
	}

	public void OnItemClicked(SwappableItem item) {
		isMovingItem = true;

		var index = item.GetIndex();
		AddPlaceholder(item, index);

		OnIsMovingItem();
	}

	public void OnItemMoved(SwappableItem item) {

	}

	public void OnItemReleased(SwappableItem item) {
		isMovingItem = false;

		var index = placeholder.GetIndex();
		RemovePlaceholder();
		MoveChild(item, index);

		OnIsMovingItem();
	}

	void OnIsMovingItem() {
		var children = GetChildren();
		foreach (var child in children) {
			if (child is SwappableItem swappableEffect) {
				swappableEffect.OnIsMovingItem(isMovingItem);
			}
		}
	}

	// public override void _Input(InputEvent @event) {
    //     base._Input(@event);
		
		

	// 	var children = GetChildren();
	// 	foreach (var child in children) {
	// 		child._Input(@event);
	// 	}
	// }

    public override void _UnhandledInput(InputEvent @event) {
        base._UnhandledInput(@event);

		// GD.Print(@event);

		// Input.ParseInputEvent(@event);

		// foreach (var child in this.GetAllChildren()) {
		// 	(child as Control)._GuiInput(@event);
		// }
    }
}
