using Godot;
using System;

public partial class RewardsContainer : Control
{
	[Export] CheckButton checkBox;
	[Export] LineEdit lineEdit;

	public void SetState(string state) {
		switch(state) {
			case "Disabled":
				SetActive(false);
				break;

			case "Enabled":
				SetActive(true, true);
				break;

			case "Builtin":
				SetActive(true, false);
				break;
		}
	}

	void SetActive(bool active, bool customizable = false) {
		lineEdit.Editable = active;

		checkBox.Disabled = !active || !customizable;
	}
}
