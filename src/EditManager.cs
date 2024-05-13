using Godot;
using Godot.Collections;
using System;

public partial class EditManager : Node
{
	public static EditManager instance;

	[Export] public FileDialogHandler fileDialog;

	[Export] public TextureRect bleedZonesMask;

	[ExportGroup("Stats")]
	string curStatsString;
	bool curCustomStats;
	[Export] public StatPanel hpStat;
	[Export] public StatPanel diceStat;
	[Export] public StatPanel atkStat;
	[Export] public StatsOptionButton statsOptionButton;

	public bool bleedZonesVisible = false;

    public override void _EnterTree() {
		EditManager.instance = this;

		var minRatio = 0.75f;

		int width = (int) Godot.ProjectSettings.GetSetting("display/window/size/viewport_width");
        int height = (int) Godot.ProjectSettings.GetSetting("display/window/size/viewport_height");
        GetTree().Root.MinSize = new Vector2I((int) (width * minRatio), (int) (height * minRatio));
    }

    public override void _Ready() {
		base._Ready();

	}

	public override void _Process(double delta) {
		base._Process(delta);
	}

	public void SetStats(string statsString, bool hasCustomStats, bool fromOptionButton = false) {
		curStatsString = statsString;
		curCustomStats = hasCustomStats;

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

		if (!fromOptionButton) {
			statsOptionButton.SelectByName(statsString);
		}
	}

	public void SetCustomStats(bool enabled) {
		statsOptionButton.SetActive(enabled);
	}

	public void UpdateStats() {
		Card.instance.setStats(curStatsString, hpStat.spinBox.Value, diceStat.spinBox.Value, atkStat.spinBox.Value, curCustomStats);
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
}
