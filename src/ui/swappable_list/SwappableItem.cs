using Godot;
using System;

public partial class SwappableItem : Control
{
    [Export] public Button button;
    [Export] public Button trashButton;
    [Export] public Control content;

    public SwappableList list;

    public bool buttonDown;

    public override void _Process(double delta) {
        base._Process(delta);

        if (buttonDown) {
            WhileButtonDown();
        }

        ResizeItem();
    }

    public void ResizeItem() {
        if (button == null || content == null || trashButton == null) {
            return;
        }

        if (button.Size.Y != content.Size.Y) {
            button.Size = new Vector2(button.Size.X, content.Size.Y);
            trashButton.Size = new Vector2(trashButton.Size.X, content.Size.Y);
        }

        CustomMinimumSize = content.Size;
    }

    public virtual void OnButtonDown() {
        buttonDown = true;

        // MouseFilter = MouseFilterEnum.Ignore;
        // foreach (var child in this.GetAllChildren()) {
        //     if (child is Control control) {

        //         GD.Print("set mouse filter of " + control.Name + " to Ignore");

        //         control.MouseFilter = MouseFilterEnum.Ignore;
        //     }
        // }

        Visible = false;

        list.OnItemClicked(this);

        this.ChangeOwner(GetTree().Root, true);
    }

    public void WhileButtonDown() {
        var mousePos = GetGlobalMousePosition();
        GlobalPosition = mousePos - new Vector2(8, button.Size.Y / 2);
    }

    public virtual void OnButtonUp() {
        buttonDown = false;

        // MouseFilter = MouseFilterEnum.Stop;
        // foreach (var child in this.GetAllChildren()) {
        //     if (child is Control control) {

        //         GD.Print("set mouse filter of " + control.Name + " to Stop");

        //         control.MouseFilter = MouseFilterEnum.Stop;
        //     }
        // }

        Visible = true;

        this.ChangeOwner(list, true);

        list.OnItemReleased(this);
    }
    
    public override void _Input(InputEvent @event) {
        base._Input(@event);

        if (@event is InputEventMouseButton eventMouseButton) {
            if (eventMouseButton.ButtonIndex == MouseButton.Left && !eventMouseButton.Pressed) {
                OnMouseUp();
            }
        }
    }

    public void OnMouseUp() {
        if (buttonDown) {
            OnButtonUp();
        }
    }

    public virtual void OnIsMovingItem(bool isMovingItem) {
        if (content != null) {
            content.MouseFilter = isMovingItem ? MouseFilterEnum.Ignore : MouseFilterEnum.Stop;
        }
    }

    public void Trash() {
        GetParent().RemoveChild(this);
		Dispose();
    }
}
