using Godot;
using System;

public partial class SelectionProp : Control
{
	public override void _Ready() {
        base._Ready();

        Card.instance.SelectedArtChanged += SelectedArtChanged;

        SetEnabled(false);
	}

    public virtual void SelectedArtChanged(MoveableArt art = null) {
        HandleArtProperties(art);
    }

    public virtual void HandleArtProperties(MoveableArt art) {}

    public virtual void SetEnabled(bool enabled) {
        foreach (var child in GetChildren()) {
            if (child is Button button) {
                button.Disabled = !enabled;
            }
            else if (child is LineEdit lineEdit) {
                lineEdit.Editable = enabled;
            }
        }
    }
}