using Godot;
using System;

public partial class DescBase : Control
{
    [Export] public int padding;

    public DescContainer container;

    public virtual void SetPadding(int value) {
        padding = value;

        SaveManager.instance.OnNeedSaveAction();
    }
}
