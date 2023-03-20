using Godot;
using System;

public partial class SwappableItem : Control
{
    [Export] public Button button;
    [Export] public Button trashButton;
    [Export] public Control content;

    public SwappableList list;

    public bool buttonDown;

    public Control descCorrespondant;

    public override void _Ready() {
        base._Ready();

        descCorrespondant = CreateDescCorrespondant();
    }

    public virtual Control CreateDescCorrespondant() {
        return null;
    }

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

    public virtual void OnListRearranged() {
        descCorrespondant.GetParent().MoveChild(descCorrespondant, GetIndex());
    }

    public void Trash() {
        if (descCorrespondant != null) {
            descCorrespondant.GetParent().RemoveChild(descCorrespondant);
            descCorrespondant.Dispose();
        }
        
        GetParent().RemoveChild(this);
		Dispose();
    }
}
