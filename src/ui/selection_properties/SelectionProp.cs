using Godot;
using System;

public partial class SelectionProp : Control
{
    public MoveableArt selectedArt;

	public override void _Ready() {
        base._Ready();

        Card.instance.SelectedArtChanged += SelectedArtChanged;

        SetEnabled(false);
	}

    public virtual void SelectedArtChanged(MoveableArt art = null) {
        if (selectedArt != null) {
            DisconnectSignals();
        }

        selectedArt = art;
        HandleArtProperties(art);

        ConnectSignals();
    }

    public virtual void DisconnectSignals() {}

    public virtual void ConnectSignals() {}

    public virtual void HandleArtProperties(MoveableArt art) {}

    public virtual void SetEnabled(bool enabled) {
        foreach (var child in GetChildren()) {
            if (child is Button button) {
                button.Disabled = !enabled;
            }
            else if (child is LineEdit lineEdit) {
                lineEdit.Editable = enabled;
            }
            else if (child is SpinBox spinBox) {
                spinBox.Editable = enabled;
            }
            else if (child is Slider slider) {
                slider.Editable = enabled;
            }
            else if (child is Label label) {
                label.Modulate = enabled ? new Color(0xFFFFFFFF) : new Color(0x808080FF);
            }
        }
    }
}