using Godot;
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
		EditManager.instance.SetStats(GetItemText(index));
	}
}
