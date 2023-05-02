using Godot;
using System;

public partial class Export : PopupMenu
{
	public void OnIndexPressed(int index) {
		switch (GetItemText(index)) {
			case "Export" :
				SaveManager.instance.Export();
			break;
		}
	}
}
