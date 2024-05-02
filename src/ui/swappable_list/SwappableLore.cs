using Godot;
using System;

public partial class SwappableLore : SwappableEffect
{
    public override DescBase CreateDescCorrespondant() {
        descEffect = Card.instance.AddText(true);
        return descEffect;
    }
}
