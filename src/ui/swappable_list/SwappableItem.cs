using Godot;
using System;

public partial class SwappableItem : Control
{
    [Export] public string type;

    [Export] public Button button;
    [Export] public Button trashButton;
    [Export] public Button settingsButton;
    [Export] public Control content;

    public SwappableList list;

    public bool buttonDown;

    public DescBase descCorrespondant;

    float baseCustomMinimumHeight;

    // Settings
    [Export] public SpinBox paddingSpinBox;
    public int padding {
        get {
            return descCorrespondant.padding;
        }
        set {
            descCorrespondant.SetPadding(value);
        }
    }

    public override void _Ready() {
        base._Ready();

        descCorrespondant = CreateDescCorrespondant();

        baseCustomMinimumHeight = CustomMinimumSize.Y;
    }

    public virtual DescBase CreateDescCorrespondant() {
        return null;
    }

    public override void _Process(double delta) {
        base._Process(delta);

        if (buttonDown) {
            WhileButtonDown();
        }
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
            descCorrespondant.Trash();
        }
        
        GetParent().RemoveChild(this);
		Dispose();
    }

    public void OnMainControlResized() {
        Size = new Vector2(Size.X, content.Size.Y);
        CustomMinimumSize = new Vector2(CustomMinimumSize.X, Mathf.Max(baseCustomMinimumHeight, content.Size.Y));
    }

    // Settings
    public void OnPaddingChanged(float value) {
        padding = (int) value;
    }
}
