using Godot;
using System;

public partial class RotationProps : SelectionProp
{
	[Export] SpinBox spinBox;

	public override void HandleArtProperties(MoveableArt art) {
		SetEnabled(art.canResetRotation);
		OnRotationChanged(art.RotationDegrees);
	}

	public override void ConnectSignals() {
		selectedArt.RotationChanged += OnRotationChanged;
	}

	public override void DisconnectSignals() {
		selectedArt.RotationChanged -= OnRotationChanged;
	}

	void OnRotationChanged(float rotation) {
		SetSpinBoxValue(rotation);
	}

	void OnSpinBoxValueChanged(float value) {
		selectedArt.SetRotationDegrees(value);
	}

	void SetSpinBoxValue(float value) {
		spinBox.Value = value;
	}

	void OnPressed() {
		selectedArt.SetRotationDegrees(0);
	}
}
