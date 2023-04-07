using Godot;
using System;

public partial class File : PopupMenu
{
	public void OnIndexPressed(int index) {
		switch (GetItemText(index)) {
			case "New" :
				SaveManager.instance.Reset();
			break;

			case "Load..." :
				SaveManager.instance.Load();
			break;

			case "Save (ctrl+s)" :
				SaveManager.instance.Save();
			break;

			case "Save as..." :
				SaveManager.instance.SaveAs();
			break;
		}
	}
}
