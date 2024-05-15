using Godot;
using System;

public partial class RewardsContainer : Control
{
	[Export] RewardsCheckButton checkButton;
	[Export] LineEdit lineEdit;

	bool curBuiltIn;

	public void SetState(string state) {
		switch(state) {
			case "Disabled":
				SetActive(false);
				break;

			case "Enabled":
				SetActive(true, false);
				break;

			case "Builtin":
				SetActive(true, true);
				break;
		}
	}

	void SetActive(bool enabled, bool builtin = false) {
		curBuiltIn = builtin;

		lineEdit.Editable = enabled;

		checkButton.Disabled = !enabled || builtin;

		var finalEnabled = enabled && (builtin || checkButton.ButtonPressed);

		Card.instance.SetRewardsEnabled(finalEnabled, builtin);
	}

	public void OnCheckButtonToggled(bool buttonPressed) {
		Card.instance.SetRewardsEnabled(buttonPressed, curBuiltIn);
	}
}
