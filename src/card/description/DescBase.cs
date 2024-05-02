using Godot;
using System;

public partial class DescBase : Control
{
    [Signal] public delegate void OnAnySizeChangeEventHandler();

    [Export] public int padding;

    public DescContainer container;

    public virtual void SetPadding(int value) {
        padding = value;

        SaveManager.instance.OnNeedSaveAction();

        EmitSignal(SignalName.OnAnySizeChange);
    }

    public virtual void Trash() {
        GetParent().RemoveChild(this);
        Dispose();
    }
}
