using Godot;
using System;

public partial class Export : PopupMenu
{
	[Export] Window exportWindow;

	public void OnIndexPressed(int index) {
		switch (GetItemText(index)) {
			case "Quick export" :
				QuickExport();
			break;

			case "Export..." :
				ShowExportWindow();
			break;
		}
	}

	async void QuickExport() {
		await SaveManager.instance.ExportFile(true, true);
	}

	void ShowExportWindow() {
		if (!exportWindow.Visible) {
			exportWindow.PopupCentered();
		}
	}
}
