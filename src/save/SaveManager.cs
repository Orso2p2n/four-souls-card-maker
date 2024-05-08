using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class SaveManager : Node
{
    static public SaveManager instance;

    string _savePath;
    public string savePath{
        get {
            return _savePath;
        }
        set {
            _savePath = value;
            saveDir = System.IO.Path.GetDirectoryName(value);
            saveName = System.IO.Path.GetFileNameWithoutExtension(value);
        }
    }

    string saveDir;
    string saveName = "new_card";

    public bool needToSave;

    [Export] Label saveLabel;
    [Export] Viewport cardViewport;

    public override void _Ready() {
        base._Ready();

        instance = this;
    }

    public override void _Input(InputEvent @event) {
		if (@event.IsActionPressed("Save")) {
			Save();
		}

		if (@event.IsActionPressed("SaveAs")) {
			SaveAs();
		}
	}

    public void Reset() {
        GetTree().ReloadCurrentScene();
    }

    
    public string GlobalPathToLocal(string path) {
        if (savePath == null) {
            return path;
        }

        var localPath = System.IO.Path.GetRelativePath(saveDir, path);

        return localPath;
    }

    public string LocalPathToGlobal(string path) {
        if (savePath == null) {
            return path;
        }

        var globalPath = saveDir + "\\" + path;

        return globalPath;
    }

    public void UpdateSaveLabel() {
        saveLabel.Text = saveName;

        if (needToSave) {
            saveLabel.Text += "*";
        }
    }

    public void OnNeedSaveAction() {
        needToSave = true;
        UpdateSaveLabel();
    }

    public void ResetNeedToSave() {
        needToSave = false;
        UpdateSaveLabel();
    }

    // --- SAVE ---
    public void Save() {
        if (savePath == null) {
            SaveAs();
            return;
        }

        SaveAtPath(savePath);
    }

    public void SaveAs() {
        EditManager.instance.WriteSaveFileDialog(new Callable(this, "SaveAtPath"));
    }

    void SaveAtPath(string path) {
        if (path != savePath) {
            savePath = path;
        }

        using var saveGame = FileAccess.Open(path, FileAccess.ModeFlags.Write);

        saveGame.StoreLine("{");

        var saveNodes = GetTree().GetNodesInGroup("Save");
        foreach (Node saveNode in saveNodes) {
            if (!saveNode.HasMethod("Save")) {
                GD.Print($"Save node '{saveNode.Name}' is missing a Save() function, skipped");
                continue;
            }

            var dict = saveNode.Call("Save");

            var jsonString = Json.Stringify(dict);
            jsonString = $"\"{saveNode.Name}\":{jsonString}";

            if (saveNode != saveNodes[saveNodes.Count-1])  {
                jsonString += ",";
            }

            saveGame.StoreLine(jsonString);
        }

        saveGame.StoreLine("}");

        ResetNeedToSave();
    }

    // --- LOAD ---
    public void Load() {
        EditManager.instance.LoadSaveFileDialog(new Callable(this, "LoadPathCallable"));
    }
    
    void LoadPathCallable(string path) {
        _ = LoadPath(path);
    }

    public async Task LoadPath(string path) {
        if (path != savePath) {
            savePath = path;
        }

        using var saveGame = FileAccess.Open(path, FileAccess.ModeFlags.Read);

        var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        var jsonString = file.GetAsText();

        var saveNodes = GetTree().GetNodesInGroup("Save");

        // Creates the helper class to interact with JSON
        var json = new Json();
        var parseResult = json.Parse(jsonString);
        if (parseResult != Error.Ok) {
            GD.Print($"JSON Parse Error: {json.GetErrorMessage()} in {jsonString} at line {json.GetErrorLine()}");
            return;
        }

        // Get the data from the JSON object
        var allData = new Dictionary<string, Variant>((Dictionary) json.Data);

        foreach (Node saveNode in saveNodes) {
            var method = saveNode.GetType().GetMethod("Load");
            if (method == null) {
                GD.Print($"Save node '{saveNode.Name}' is missing a Load() function, skipped");
                continue;
            }

            var data = (Dictionary) allData[saveNode.Name];
            if (data == null) {
                GD.Print($"Save node '{saveNode.Name}' doesn't have data in the loaded file");
                continue;
            }

            saveNode.CallDeferred("Load", data);
        }

        await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

        ResetNeedToSave();
    }

    // --- EXPORT ---
    public async Task ExportFolder(bool exportTabletop, bool exportPrinting, string folderPath, string exportLocation = null, bool createTabletopFolder = false, bool createPrintingFolder = false) {
        GD.Print(folderPath);

        var cardPaths = GetCardsInDir(folderPath);

        foreach (var path in cardPaths) {
            await ExportFile(exportTabletop, exportPrinting, path, exportLocation, createTabletopFolder, createPrintingFolder);
        }
    }

    Array<string> GetCardsInDir(string dirPath) {

        Array<string> cardPaths = new Array<string>();

        var directory = DirAccess.Open(dirPath);

        if (directory == null) {
            GD.PrintErr("An error occured when trying to access the path at " + dirPath + ".");
            return cardPaths;
        }

        directory.ListDirBegin();

        string curFileName = directory.GetNext();
        while (curFileName != "") {
            
            if (directory.CurrentIsDir()) {
                var curDirCards = GetCardsInDir(dirPath + "\\" + curFileName);
                cardPaths.AddRange(curDirCards);
            }
            else {
                if (curFileName.EndsWith(".fscard")) {
                    cardPaths.Add(dirPath + "\\" + curFileName);
                }
            }

            curFileName = directory.GetNext();
        }

        return cardPaths;
    }

    public async Task ExportFile(bool exportTabletop, bool exportPrinting, string filePath = null, string exportLocation = null, bool createTabletopFolder = false, bool createPrintingFolder = false) {
        if (cardViewport == null) {
            return;
        }

        if (filePath != null && filePath != savePath) {
            if (savePath != null) {
                Save();
            }
            
            await LoadPath(filePath);
        }

        if (exportLocation == null) {
            exportLocation = saveDir;
        }

        if (savePath == null || savePath == "") {
            return;
        }

        var oldShowBleedZones = EditManager.instance.bleedZonesVisible;

        if (exportTabletop) {
            await Export(false, exportLocation, createTabletopFolder, "tabletop");
        }

        if (exportPrinting) {
            await Export(true, exportLocation, createPrintingFolder, "print");
        }

        EditManager.instance.SetBleedZones(oldShowBleedZones);
    }

    async Task Export(bool showBleedZones, string exportLocation, bool createFolder, string suffix) {
        EditManager.instance.SetBleedZones(showBleedZones);

        await ToSignal(RenderingServer.Singleton, "frame_post_draw");

        var img = cardViewport.GetTexture().GetImage();
        if (img == null) {
            return;
        }
        
        // Export
        var fileName = saveName;

        if (createFolder) {
            exportLocation += "\\" + suffix;
            DirAccess.MakeDirAbsolute(exportLocation);
        }

        fileName += "_" + suffix + ".png";

        var exportPath = exportLocation + "\\" + fileName;
        var error = img.SavePng(exportPath);
    }
}
