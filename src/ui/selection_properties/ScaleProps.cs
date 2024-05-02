using Godot;
using System;

public partial class ScaleProps : SelectionProp
{
	[Export] SpinBox spinBox;

	public override void HandleArtProperties(MoveableArt art) {
		SetEnabled(art.canResetScale);
		OnScaleChanged(art.Scale.X);
	}

	public override void ConnectSignals() {
		selectedArt.ScaleChanged += OnScaleChanged;
	}

	public override void DisconnectSignals() {
		selectedArt.ScaleChanged -= OnScaleChanged;
	}

	void OnScaleChanged(float scale) {
		SetSpinBoxValue(scale * 100);
	}

	void OnSpinBoxValueChanged(float value) {
		selectedArt.SetScale(value / 100);
	}

	void SetSpinBoxValue(float value) {
		spinBox.Value = value;
	}

	void OnPressed() {
		selectedArt.SetScale(1);
	}
}
