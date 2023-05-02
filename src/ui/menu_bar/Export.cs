using Godot;
using System;

public partial class Export : PopupMenu
{
	public void OnIndexPressed(int index) {
		switch (GetItemText(index)) {
			case "Export for print" :
				SaveManager.instance.Export(true);
			break;

			case "Export for tabletop" :
				SaveManager.instance.Export(false);
			break;
		}
	}
}
