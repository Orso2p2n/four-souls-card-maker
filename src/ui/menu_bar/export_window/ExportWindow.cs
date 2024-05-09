using Godot;
using System;

public partial class ExportWindow : Window
{
    [Export] ExportCheckbox exportTabletopCheckbox;
    [Export] ExportCheckbox createTabletopFolderCheckbox;

    [Export] ExportCheckbox exportPrintingCheckbox;
    [Export] ExportCheckbox createPrintingFolderCheckbox;
    
    [Export] Button fileSelectionButton;
    [Export] Button locationButton;
    [Export] Button exportButton;
 
    bool exportTabletop;
    bool createTabletopFolder;

    bool exportPrinting;
    bool createPrintingFolder;

    string selectedFileOrFolderPath = null;
    bool exportFolder;
    string exportLocationPath = null;

    public override void _Ready() {
        base._Ready();
    
        // Connect checkbox signals
        exportTabletop = exportTabletopCheckbox.ButtonPressed;
        createTabletopFolder = createTabletopFolderCheckbox.ButtonPressed;
        exportPrinting = exportPrintingCheckbox.ButtonPressed;
        createPrintingFolder = createPrintingFolderCheckbox.ButtonPressed;

        exportTabletopCheckbox.Toggled += ExportTabletopCheckboxToggled;
        createTabletopFolderCheckbox.Toggled += CreateTabletopFolderCheckboxToggled;
        exportPrintingCheckbox.Toggled += ExportPrintingCheckboxToggled;
        createPrintingFolderCheckbox.Toggled += CreatePrintingFolderCheckboxToggled;

        // Connect button signals
        fileSelectionButton.Pressed += FileSelectionButtonPressed;
        locationButton.Pressed += LocationButtonPressed;
        exportButton.Pressed += ExportButtonPressed;
    }

    // Checkbox signals
    void ExportTabletopCheckboxToggled(bool buttonPressed) {
        exportTabletop = buttonPressed;
    }

    void CreateTabletopFolderCheckboxToggled(bool buttonPressed) {
        createTabletopFolder = buttonPressed;
    }

    void ExportPrintingCheckboxToggled(bool buttonPressed) {
        exportPrinting = buttonPressed;
    }

    void CreatePrintingFolderCheckboxToggled(bool buttonPressed) {
        createPrintingFolder = buttonPressed;
    }

    // Button signals
    void FileSelectionButtonPressed() {
        EditManager.instance.fileDialog.SelectFileOrFolderDialog(new Callable(this, "OnFileSelected"), new Callable(this, "OnFolderSelected"));
    }

    void LocationButtonPressed() {
        EditManager.instance.fileDialog.SelectFolderDialog(new Callable(this, "OnExportLocationSelected"));
    }

    void ExportButtonPressed() {
        Export();
    }
    
    public void OnCloseRequested() {
		Visible = false;
	}

    // Files and folder selected
    void OnFileSelected(string path) {
        GD.Print(path);

        fileSelectionButton.Text = path;

        selectedFileOrFolderPath = path;
        exportFolder = false;
    }

    void OnFolderSelected(string path) {
        GD.Print(path);

        fileSelectionButton.Text = path;

        selectedFileOrFolderPath = path;
        exportFolder = true;
    }
    
    void OnExportLocationSelected(string path) {
        GD.Print(path);

        locationButton.Text = path;

        exportLocationPath = path;
    }

    // Final export
    async void Export() {
        if (exportFolder) {
            await SaveManager.instance.ExportFolder(exportTabletop, exportPrinting, selectedFileOrFolderPath, exportLocationPath, createTabletopFolder, createPrintingFolder);
        }
        else {
            await SaveManager.instance.ExportFile(exportTabletop, exportPrinting, selectedFileOrFolderPath, exportLocationPath, createTabletopFolder, createPrintingFolder);
        }

        OnCloseRequested();
    }
}
