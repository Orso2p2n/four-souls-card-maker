using Godot;
using Godot.Collections;
using System;

public partial class CardMainType : CardType
{
    [Export(PropertyHint.Enum,"None,HP DICE ATK,HP ATK")] public string stats = "None";
    [Export] public Array<CardBackground> backgrounds = new Array<CardBackground>();
    [Export] public Array<CardForeground> foregrounds = new Array<CardForeground>();
}
