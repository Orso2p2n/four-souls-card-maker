using Godot;
using System;

[Tool]
public partial class ControlPlus : Control
{
    public override void _Process(double delta) {
        base._Process(delta);

        if (Engine.IsEditorHint()) {
            EditorProcess(delta);
        }
    }

    public virtual void EditorProcess(double delta) {

    }
}
