using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class SaveManager : Node
{
    static public SaveManager instance;

    public string savePath;

    [ExportGroup("Type Menus")]
    [Export] MainTypeMenu mainTypeMenu;
    [Export] BackgroundMenu backgroundMenu;
    [Export] BorderMenu borderMenu;
    [Export] ForegroundMenu foregroundMenu;

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
    }

    // --- LOAD ---
    public void Load() {
        EditManager.instance.LoadSaveFileDialog(new Callable(this, "LoadPathCallable"));
    }
    
    void LoadPathCallable(string path) {
        Task task = LoadPath(path);
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

            var task = (Task) method.Invoke(saveNode, new object[] {data});

            if (task == null) {
                continue;
            }

            await task.ConfigureAwait(false);
        }
    }
}
