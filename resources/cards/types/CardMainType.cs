using Godot;
using Godot.Collections;
using System;

public partial class CardMainType : CardType
{
    [Export] public Array<CardSubType> subTypes = new Array<CardSubType>();
}
