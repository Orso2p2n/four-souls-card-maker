using Godot;
using System;

public partial class SwappableSeparator : SwappableItem
{
    public OptionButton lineSelection;

    public DescLine descLine;

    public int curIndex {
        get {
            return lineSelection.Selected;
        }
        set {

            if (lineSelection.Selected != value) {
                lineSelection.Select(value);
            }

            switch (value) {
                case 0 : descLine.SetAlpha(1.0f); break;
                case 1 : descLine.SetAlpha(0.5f); break;
                case 2 : descLine.SetAlpha(0.0f); break;
            }
        }
    }

    public override void _Ready() {
        base._Ready();

        lineSelection = content as OptionButton;
    }

    public override DescBase CreateDescCorrespondant() {
        descLine = Card.instance.AddLine();
        return descLine;
    }

    void OnItemSelected(int index) {
        curIndex = index;
    }
}
