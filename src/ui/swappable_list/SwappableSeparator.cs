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
        var light = (index == 1);

        descLine.SetState(light);
    }
}
