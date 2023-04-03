using Godot;
using System;

public partial class SwappableEffect : SwappableItem
{
    public TextEdit textEdit;

    [Export] public SpinBox paddingEdit;
    [Export] public SpinBox scaleEdit;

    public DescEffect descEffect;

    public override void _Ready() {
        base._Ready();

        textEdit = content as TextEdit;
    }

    public override DescBase CreateDescCorrespondant() {
        descEffect = Card.instance.AddEffect();
        return descEffect;
    }

    public override void OnButtonDown() {
        base.OnButtonDown();

        textEdit.Editable = false;
        textEdit.SelectingEnabled = false;
    }

    public override void OnButtonUp() {
        base.OnButtonUp();

        textEdit.Editable = true;
        textEdit.SelectingEnabled = true;
    }

    public override void OnIsMovingItem(bool isMovingItem) {
        base.OnIsMovingItem(isMovingItem);

        if (paddingEdit != null) {
            paddingEdit.MouseFilter = isMovingItem ? MouseFilterEnum.Ignore : MouseFilterEnum.Stop;
        }

        if (scaleEdit != null) {
            scaleEdit.MouseFilter = isMovingItem ? MouseFilterEnum.Ignore : MouseFilterEnum.Stop;
        }
    }

    void OnTextChanged() {
        descEffect.SetText(textEdit.Text);
    }
}
