using Godot;
using Godot.Collections;
using System;

public partial class DescContainer : VBoxContainer
{
	[Signal] public delegate void ResizedWhileFittingChildrenEventHandler();

	Vector2 initialPos;
	Vector2 initialSize;

	Vector2 targetPos;
	Vector2 targetSize;

	float curTextScale = 1.0f;

	bool isCurrentlyFittingChildren = false;

	public override void _Ready() {
		initialPos = Position;
		initialSize = Size;
	}

	public override void _Process(double delta) {
		base._Process(delta);

		if (!isCurrentlyFittingChildren) {
			ResetHeight();
		}
	}

	public void SetOffsets(float topOffset, float botOffset) {
		Position = targetPos = initialPos + new Vector2(0, topOffset);
		targetSize = initialSize - new Vector2(0, topOffset + botOffset);
	}

	public void OnAddText(DescEffect addedText) {
		addedText.SetSystemScale(curTextScale);
	}

	void ResetHeight() {
		Size = new Vector2(targetSize.X, 0);
	}


	public void OnResized() {
		if (isCurrentlyFittingChildren) {
			EmitSignal(SignalName.ResizedWhileFittingChildren);
			return;
		}

		UpdateChildrenFit();
		var middlePoint = targetSize.Y / 2;
		Position = targetPos + new Vector2(0, middlePoint - Size.Y/2);
	}

	async void UpdateChildrenFit() {
		if (Size.Y <= targetSize.Y) {
			return;
		}

		isCurrentlyFittingChildren = true;

		GD.Print("children bigger than desccontainer");

		var lines = GetLineChildren();
		var texts = GetTextChildren();

		var i = 0;
		while (Size.Y > targetSize.Y && i < 99) {
			curTextScale -= 0.05f;

			foreach (var text in texts) {
				text.SetSystemScale(curTextScale);
			}

			ResetHeight();

			await ToSignal(this, "sort_children");

			// ResetHeight();

			i++;
		}

		isCurrentlyFittingChildren = false;
	}

	Array<DescEffect> GetTextChildren() {
		Array<DescEffect> descEffects = new Array<DescEffect>();

		foreach (var child in GetChildren()) {
			if (child is DescEffect descEffect) {
				descEffects.Add(descEffect);
			}
		}

		return descEffects;
	}

	Array<DescLine> GetLineChildren() {
		Array<DescLine> descLines = new Array<DescLine>();

		foreach (var child in GetChildren()) {
			if (child is DescLine descLine) {
				descLines.Add(descLine);
			}
		}

		return descLines;
	}
}
