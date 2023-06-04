using Godot;
using System;

public partial class ScaleProps : SelectionProp
{
	[Export] Label label;
	[Export] Slider slider;

	float scaleStep;

	public override void HandleArtProperties(MoveableArt art) {
		SetEnabled(art.canResetScale);
		SetSliderValue(art.Scale.X);
		slider.Step = scaleStep;
	}

	public override void ConnectSignals() {
		selectedArt.ScaleChanged += OnScaleChanged;
	}

	public override void DisconnectSignals() {
		selectedArt.ScaleChanged -= OnScaleChanged;
	}

	void OnScaleChanged(float scale) {
		SetSliderValue(scale);
	}

	void OnSliderValueChanged(float value) {
		selectedArt.SetScale(value);
	}

	void SetSliderValue(float value) {
		slider.Value = value;
		label.Text = "Scale: " + value;
	}

	void OnPressed() {
		selectedArt.SetScale(1);
	}
}
