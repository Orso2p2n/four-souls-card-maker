using Godot;
using System;

public partial class SwappableSeparator : SwappableItem
{
    public OptionButton lineSelection;

    public DescLine descLine;

    public override void _Ready() {
        base._Ready();

        lineSelection = content as OptionButton;
    }

    public override Control CreateDescCorrespondant() {
        descLine = Card.instance.AddLine();
        return descLine;
    }

    void OnItemSelected(int index) {
        switch (index) {
            case 0 : descLine.SetAlpha(1.0f); break;
            case 1 : descLine.SetAlpha(0.5f); break;
            case 2 : descLine.SetAlpha(0.0f); break;
        }
    }
}
