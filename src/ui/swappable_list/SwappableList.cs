using Godot;
using Godot.Collections;
using System;

public partial class SwappableList : VBoxContainer
{
	Control placeholder;
	bool isMovingItem;

	SwappableItem lastItemMovedUp;
	SwappableItem lastItemMovedDown;

	[Export] Dictionary<string,PackedScene> scenes = new Dictionary<string,PackedScene>{
		{"Effect", new PackedScene()},
		{"Separator", new PackedScene()},
		{"Lore", new PackedScene()},
		{"Placeholder", new PackedScene()}
	};

	[Signal] public delegate void ListChangedEventHandler();

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
		var itemIndex = item.GetIndex();
		var placeholderIndex = placeholder.GetIndex();

		GD.Print("mouse pos: " + eventMouseMotion.Position.Y + ", centerY: " + centerY + ", index: " + itemIndex);

		if (eventMouseMotion.Position.Y < centerY) {
			if (placeholderIndex <= itemIndex || item == lastItemMovedDown) {
				return;
			}

			MoveChild(placeholder, itemIndex);
			lastItemMovedDown = item;
			lastItemMovedUp = null;
		}
		else {
			if (placeholderIndex >= itemIndex || item == lastItemMovedUp) {
				return;
			}

			MoveChild(placeholder, itemIndex);
			lastItemMovedDown = null;
			lastItemMovedUp = item;
		}
	}

	public void OnItemClicked(SwappableItem item) {
		isMovingItem = true;

		var index = item.GetIndex();
		AddPlaceholder(item, index);

		OnIsMovingItem();
	}

	public void OnItemReleased(SwappableItem item) {
		isMovingItem = false;

		lastItemMovedDown = null;
		lastItemMovedUp = null;

		var index = placeholder.GetIndex();
		RemovePlaceholder();
		MoveChild(item, index);

		OnIsMovingItem();

		OnListRearranged();
	}

	void OnIsMovingItem() {
		var children = GetChildren();
		foreach (var child in children) {
			if (child is SwappableItem swappableEffect) {
				swappableEffect.OnIsMovingItem(isMovingItem);
			}
		}
	}
	
	public void OnListRearranged() {
		foreach (var child in GetChildren()) {
			(child as SwappableItem).OnListRearranged();
		}

		EmitSignal(SignalName.ListChanged);

		SaveManager.instance.OnNeedSaveAction();
	}

	// --- SAVE HANDLING ---
	public virtual Dictionary Save() {
		var dict = new Dictionary();

		var items = new Array<Dictionary>();

		foreach (var child in GetChildren()) {
			var item = child as SwappableItem;

			if (item == null) {
				continue;
			}

			var values = new Dictionary();

			values.Add("Type", item.type);

			if (item is SwappableSeparator separator) {
				values.Add("Index", separator.curIndex);
			}

			if (item is SwappableEffect effect) {
				values.Add("Text", effect.textEdit.Text);
				values.Add("Scale", effect.scale);
				values.Add("BoundsMul", effect.boundsMul);
				values.Add("LineSpacing", effect.lineSpacing);
				values.Add("CharSpacing", effect.charSpacing);
			}

			values.Add("Padding", item.padding);

			items.Add(values);
		}

		dict.Add("Items", items);

		return dict;
	}

	public virtual void Load(Dictionary data) {
		foreach (var child in GetChildren()) {
			if (child is SwappableItem item) {
				item.Trash();
				continue;
			}

			RemoveChild(child);
			child.Dispose();
		}

		var items = (Array<Dictionary>) data["Items"];

		foreach (Dictionary item in items) {
			var addedItem = (SwappableItem) AddItem((string) item["Type"]);

			addedItem.padding = (int) item["Padding"];

			if (addedItem is SwappableSeparator separator) {
				separator.curIndex = (int) item["Index"];
				continue;
			}

			if (addedItem is SwappableEffect effect) {
				var text = (string) item["Text"];
				effect.textEdit.Text = text;
				effect.OnTextChanged();

				effect.scale       = (float) item["Scale"];
				effect.boundsMul   = (float) item["BoundsMul"];
				effect.lineSpacing = (int) item["LineSpacing"];
				effect.charSpacing = (int) item["CharSpacing"];
				continue;
			}
		}
	}
}
