using Godot;
using Godot.Collections;
using System;

public partial class CardMainType : MenuItem
{
    [Export(PropertyHint.Enum,"None,Monster,Character")] public string stats = "None";
    [Export] public bool customStats = false;
    [Export] public Array<CardBackground>   backgrounds = new Array<CardBackground>();
    [Export] public Array<CardForeground>   foregrounds = new Array<CardForeground>();
    [Export] public Array<CardBorder>       borders     = new Array<CardBorder>();

    [Export] public bool canHaveStartingItem = false;

    [ExportGroup("Description Offset")]
    [Export] public float descOffsetTop = 0f;
    [Export] public float descOffsetBot = 0f;
}
