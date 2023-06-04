using Godot;
using Godot.Collections;
using System;

public partial class EditManager : Node
{
	public static EditManager instance;

	[Export] public FileDialog fileDialog;
	Callable fileDialogPathCallback;
	Callable folderDialogCallback;

	[Export] public TextureRect bleedZonesMask;
	[Export] public Control cursorShapeOverride;

	[ExportGroup("Stats")]
	string curStatsString;
	[Export] public StatPanel hpStat;
	[Export] public StatPanel diceStat;
	[Export] public StatPanel atkStat;

	public bool bleedZonesVisible = false;

	public override void _Ready() {
		base._Ready();

		EditManager.instance = this;
	}

	public override void _Process(double delta) {
		base._Process(delta);
	}

	public void SetStats(string statsString) {
		curStatsString = statsString;

		bool hasHp = false;
		bool hasDice = false;
		bool hasAtk = false;

		switch(statsString) {
			case "None": break;

			case "Monster": 
				hasHp = true;
				hasDice = true;
				hasAtk = true;
				break;

			case "Character":
				hasHp = true;
				hasAtk = true;
				break;
		}
		
		hpStat.SetActive(hasHp);
		diceStat.SetActive(hasDice);
		atkStat.SetActive(hasAtk);

		UpdateStats();
	}

	public void UpdateStats() {
		Card.instance.setStats(curStatsString, hpStat.spinBox.Value, diceStat.spinBox.Value, atkStat.spinBox.Value);
	}

	public void ToggleBleedZones() {
		SetBleedZones(!bleedZonesVisible);
	}

	public void SetBleedZones(bool show) {
		bleedZonesVisible = show;
		if (show) {
			Card.instance.ShowBleedZones();
		}
		else {
			Card.instance.HideBleedZones();
		}
	}

	public void LoadTextureFileDialog(Callable callback) {
        fileDialogPathCallback = callback;

		fileDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
		fileDialog.Filters = new string[]{"*.png, *.jpg, *.jpeg ; Supported Images"};
		fileDialog.Visible = true;
		fileDialog.FileSelected += OnTexturePathSelected;
	}

    void OnTexturePathSelected(string path) {
		var texture = LoadTextureFromPath(path);
        
        fileDialogPathCallback.Call(path, texture);

        fileDialog.FileSelected -= OnTexturePathSelected;
    }

	public Texture2D LoadTextureFromPath(string path) {
		var image = new Image();
        var error = image.Load(path);

        if (error != Error.Ok) {
            GD.PrintErr("Failed to load " + path + ". Error: " + error);
        }

        var texture = ImageTexture.CreateFromImage(image);
		return texture;
	}

	public void WriteSaveFileDialog(Callable callback) {
        fileDialogPathCallback = callback;

		fileDialog.FileMode = FileDialog.FileModeEnum.SaveFile;
		fileDialog.Filters = new string[]{"*.fscard ; Four Souls Card"};
		fileDialog.Visible = true;
		fileDialog.FileSelected += OnFileSelected;
	}

	public void LoadSaveFileDialog(Callable callback) {
        fileDialogPathCallback = callback;

		fileDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
		fileDialog.Filters = new string[]{"*.fscard ; Four Souls Card"};
		fileDialog.Visible = true;
		fileDialog.FileSelected += OnFileSelected;
	}

	public void SelectFileOrFolderDialog(Callable fileCallback, Callable folderCallback) {
        fileDialogPathCallback = fileCallback;
        folderDialogCallback = folderCallback;

		fileDialog.FileMode = FileDialog.FileModeEnum.OpenAny;
		fileDialog.Filters = new string[]{"Folder","*.fscard ; Four Souls Card"};
		fileDialog.Visible = true;
		fileDialog.FileSelected += OnFileSelected;
		fileDialog.DirSelected += OnDirSelected;
	}

	public void SelectFolderDialog(Callable callback) {
        folderDialogCallback = callback;

		fileDialog.FileMode = FileDialog.FileModeEnum.OpenDir;
		fileDialog.Filters = new string[]{"Folder"};
		fileDialog.Visible = true;
		fileDialog.DirSelected += OnDirSelected;
	}

	void OnFileSelected(string path) {
		fileDialogPathCallback.Call(path);

		fileDialog.FileSelected -= OnFileSelected;
		fileDialog.DirSelected -= OnDirSelected;
	}

	void OnDirSelected(string path) {
		folderDialogCallback.Call(path);

		fileDialog.FileSelected -= OnFileSelected;
		fileDialog.DirSelected -= OnDirSelected;
	}

	public Control.CursorShape GetCursorShape() {
		return cursorShapeOverride.MouseDefaultCursorShape;
	}

	public void SetCursorShape(Control.CursorShape cursorShape) {
		cursorShapeOverride.MouseDefaultCursorShape = cursorShape;
	}
}
