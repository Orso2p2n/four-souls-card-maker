using Godot;
using System;

public partial class PositionProps : SelectionProp
{
	[Export] SpinBox xSpinBox;
	[Export] SpinBox ySpinBox;

	public override void HandleArtProperties(MoveableArt art) {
		SetEnabled(art.canResetPosition);

		SetSpinBoxesBounds(art.minPos, art.maxPos);
		SetSpinBoxVal(xSpinBox, art.Position.X);
		SetSpinBoxVal(ySpinBox, art.Position.Y);
	}

	public override void ConnectSignals() {
		selectedArt.PositionChanged += OnPositionChanged;
	}

	public override void DisconnectSignals() {
		selectedArt.PositionChanged -= OnPositionChanged;
	}

	void OnPositionChanged(Vector2 pos) {
		SetSpinBoxVal(xSpinBox, pos.X);
		SetSpinBoxVal(ySpinBox, pos.Y);
	}

	void OnChangedX(float val) {
		selectedArt.SetX(val);
	}

	void OnChangedY(float val) {
		selectedArt.SetY(val);
	}

	void SetSpinBoxesBounds(Vector2 minPos, Vector2 maxPos) {
		xSpinBox.MinValue = minPos.X;
		xSpinBox.MaxValue = maxPos.X;

		ySpinBox.MinValue = minPos.Y;
		ySpinBox.MaxValue = maxPos.Y;
	}

	void SetSpinBoxVal(SpinBox spinBox, float val) {
		spinBox.Value = val;
	}

	void OnResetXPressed() {
		selectedArt.SetX(0);
	}

	void OnResetYPressed() {
		selectedArt.SetY(0);
	}
}
