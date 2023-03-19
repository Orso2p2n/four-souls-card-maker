using Godot;
using Godot.Collections;
using System;

public partial class CardSubType : CardType
{
    [Export] public Texture bgTexture;

    [Export] public Texture top;
    [Export] public Texture bot;
    [Export] public Texture border;
}
