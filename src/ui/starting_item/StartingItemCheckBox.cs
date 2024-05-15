using Godot;
using Godot.Collections;
using System;

public partial class StartingItemCheckBox : CheckButton
{

	public void OnToggled(bool buttonPressed) {
		if (!Disabled) {
			Card.instance.SetStartingItemVisible(buttonPressed);
		}
	}

	// --- SAVE HANDLING ---
	public virtual Dictionary Save() {
		var dict = new Dictionary();

		dict.Add("Toggled", ButtonPressed);

		return dict;
	}

	public virtual void Load(Dictionary data) {
        bool toggled = (bool) data["Toggled"];
		ButtonPressed = toggled;
    }
}
