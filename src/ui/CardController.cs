using Godot;
using System;

public partial class CardController : Node
{
	public Card card;

    public override void _Ready() {
        base._Ready();

		this.card = Card.instance;
    }
}
