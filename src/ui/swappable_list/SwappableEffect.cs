using Godot;
using System;

public partial class SwappableEffect : SwappableItem
{
    public TextEdit textEdit;

    public DescEffect descEffect;

    // Settings
    [Export] public SpinBox scaleSpinBox;
    public float scale {
        get {
            return descEffect.userScale;
        }
        set {
            if (scaleSpinBox.Value != value * 100) {
                scaleSpinBox.Value = value * 100;
            }

            descEffect.SetUserScale(value);
        }
    }

    [Export] public SpinBox boundsMulSpinBox;
    public float boundsMul {
        get {
            return descEffect.boundsMul;
        }
        set {
            if (boundsMulSpinBox.Value != value * 100) {
                boundsMulSpinBox.Value = value * 100;
            }
            
            descEffect.SetBoundsMul(value);
        }
    }

    [Export] public SpinBox lineSpacingSpinBox;
    public int lineSpacing {
        get {
            return descEffect.lineSpacingDelta;
        }
        set {
            if (lineSpacingSpinBox.Value != value) {
                lineSpacingSpinBox.Value = value;
            }
            
            descEffect.SetLineSpacing(value);
        }
    }
    
    [Export] public SpinBox charSpacingSpinBox;
    public int charSpacing {
        get {
            return descEffect.characterSpacing;
        }
        set {
            if (charSpacingSpinBox.Value != value) {
                charSpacingSpinBox.Value = value;
            }
            
            descEffect.SetCharacterSpacing(value);
        }
    }

    public override void _Ready() {
        base._Ready();

        textEdit = content as TextEdit;
    }

    public override DescBase CreateDescCorrespondant() {
        descEffect = Card.instance.AddText();
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
    }

    public void OnTextChanged() {
        descEffect.SetText(textEdit.Text, true);
    }

    // Settings
    public void OnScaleChanged(float value) {
        scale = value / 100;
    }

    public void OnBoundsChanged(float value) {
        boundsMul = value / 100;
    }

    public void OnLineSpacingChanged(float value) {
        lineSpacing = (int) value;
    }

    public void OnCharSpacingChanged(float value) {
        charSpacing = (int) value;
    }
}
