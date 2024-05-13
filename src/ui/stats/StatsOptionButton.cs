using Godot;
using Godot.Collections;
using System;

public partial class StatsOptionButton : OptionButton
{
	public void SetActive(bool active) {
		Disabled = !active;
	}

	public void SelectByName(string name) {
		for (int i = 0; i < ItemCount; i++) {
			var text = GetItemText(i);

			if (text == name) {
				Selected = i;
				return;
			}
		}
	}

	public void OnItemSelected(int index) {
		EditManager.instance.SetStats(GetItemText(index), true);
	}

	// --- SAVE HANDLING ---
	public virtual Dictionary Save() {
		var dict = new Dictionary();

		dict.Add("Selected", Selected);

		return dict;
	}

	public virtual void Load(Dictionary data) {
        int selectedId = (int) data["Selected"];
		Select(selectedId);
		OnItemSelected(selectedId);
    }
}
