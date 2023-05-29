using Godot;
using System;

public partial class ExportCheckbox : CheckBox
{
    [Export] Label linkedText;
    [Export] ExportCheckbox linkedCheckbox;

    public override void _Ready() {
        base._Ready();

        Toggled += SetEnableLinkedCheckbox;

        SetEnableLinkedCheckbox(ButtonPressed);
    }
    
    void SetEnableLinkedCheckbox(bool enabled) {
        if (linkedCheckbox == null) {
            return;
        }

        if (enabled) {
            linkedCheckbox.Enable();
        }
        else {
            linkedCheckbox.Disable();
        }
    }

    public void Disable() {
        Disabled = true;
        linkedText.Modulate = Godot.Colors.DarkGray;
        ButtonPressed = false;
    }

    public void Enable() {
        Disabled = false;
        linkedText.Modulate = Godot.Colors.White;
    }
}
