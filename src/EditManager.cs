using Godot;
using Godot.Collections;
using System;

public partial class EditManager : Node
{
	public static EditManager instance;

	[Export] public FileDialog fileDialog;
	Callable fileDialogCallback;

	[ExportGroup("Stats")]
	string curStatsString;
	[Export] public StatPanel hpStat;
	[Export] public StatPanel diceStat;
	[Export] public StatPanel atkStat;

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

	public void LoadTextureFileDialog(Callable callback) {
        fileDialogCallback = callback;

		fileDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
		fileDialog.Filters = new string[]{"*.png, *.jpg, *.jpeg ; Supported Images"};
		fileDialog.Visible = true;
		fileDialog.FileSelected += OnTexturePathSelected;
	}

    void OnTexturePathSelected(string path) {
		var texture = LoadTextureFromPath(path);
        
        fileDialogCallback.Call(path, texture);

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
        fileDialogCallback = callback;

		fileDialog.FileMode = FileDialog.FileModeEnum.SaveFile;
		fileDialog.Filters = new string[]{"*.fscard ; Four Souls Card"};
		fileDialog.Visible = true;
		fileDialog.FileSelected += OnSavePathSelected;
	}

	public void LoadSaveFileDialog(Callable callback) {
        fileDialogCallback = callback;

		fileDialog.FileMode = FileDialog.FileModeEnum.OpenFile;
		fileDialog.Filters = new string[]{"*.fscard ; Four Souls Card"};
		fileDialog.Visible = true;
		fileDialog.FileSelected += OnSavePathSelected;
	}

	void OnSavePathSelected(string path) {
		fileDialogCallback.Call(path);

		fileDialog.FileSelected -= OnSavePathSelected;
	}
}
