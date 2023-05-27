using Godot;
using System;

public partial class Help : PopupMenu
{
	[Export] Window infoWindow;

	public void OnIndexPressed(int index) {
		switch (GetItemText(index)) {
			case "Info" :
				if (!infoWindow.Visible) {
					infoWindow.PopupCentered();
				}
			break;

			case "Go to repo" :
				OS.ShellOpen("https://github.com/ZPMods/four-souls-card-maker");
			break;

			case "Report bug" :
				OS.ShellOpen("https://github.com/ZPMods/four-souls-card-maker/issues");
			break;
		}
	}
}
