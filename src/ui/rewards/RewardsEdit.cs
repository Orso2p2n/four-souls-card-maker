using Godot;
using Godot.Collections;
using System;

public partial class RewardsEdit : LineEdit
{
	public void OnTextChanged(string text) {
		Card.instance.SetRewards(text);
	}

	// --- SAVE HANDLING ---
	public virtual Dictionary Save() {
		var dict = new Dictionary();

		dict.Add("Value", Text);

		return dict;
	}

	public virtual void Load(Dictionary data) {
        Text = (string) data["Value"];
		OnTextChanged(Text);
    }
}