using Godot;
using System;

public partial class StatPanel : Control
{
	[Export] public bool active;
	[Export] public TextureRect icon;
	[Export] public SpinBox spinBox;

	public override void _Ready() {
		base._Ready();

		SetActive(active);
	}

	public void SetActive(bool active) {
		spinBox.Editable = active;
		icon.Modulate = active ? new Color(1, 1, 1, 1) : new Color(0.5f, 0.5f, 0.5f, 0.5f);
	}

	public void UpdateStat(double val) {
		EditManager.instance.UpdateStats();
	}
}
