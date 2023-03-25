using Godot;
using System;

public partial class EditManager : Node
{
    public static EditManager instance;

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
}
