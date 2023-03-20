using Godot;
using System;

public partial class SwappableLore : SwappableEffect
{
    public override Control CreateDescCorrespondant() {
        descEffect = Card.instance.AddLore();
        return descEffect;
    }
}
