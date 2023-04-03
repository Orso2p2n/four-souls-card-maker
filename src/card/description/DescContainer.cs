using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class DescContainer : VBoxContainer
{
	[Signal] public delegate void ResizedWhileFittingChildrenEventHandler();

	Vector2 initialPos;
	Vector2 initialSize;

	Vector2 targetPos;
	Vector2 targetSize;

	Vector2 oldSize;

	float curTextScale = 1.0f;

	public bool isCurrentlyFittingChildren = false;

	public override void _Ready() {
		initialPos = Position;
		initialSize = Size;
		oldSize = Size;
	}

	public override void _Process(double delta) {
		base._Process(delta);

		if (!isCurrentlyFittingChildren) {
            Task task = ResetHeight();
		}
	}

	public void SetOffsets(float topOffset, float botOffset) {
		Position = targetPos = initialPos + new Vector2(0, topOffset);
		targetSize = initialSize - new Vector2(0, topOffset + botOffset);
	}

	public void OnAddText(DescEffect addedText) {
		addedText.SetSystemScale(curTextScale);
	}

	async Task ResetHeight() {
		Size = new Vector2(targetSize.X, 0);
		await ToSignal(this, "sort_children");
	}


	public void OnResized() {
		var middlePoint = targetSize.Y / 2;
		Position = targetPos + new Vector2(0, middlePoint - Size.Y/2);

		if (isCurrentlyFittingChildren) {
			EmitSignal(SignalName.ResizedWhileFittingChildren);
			return;
		}

		UpdateChildrenFit();

		oldSize = Size;
	}

	async void UpdateChildrenFit() {
		if (Size.Y <= targetSize.Y) {
			if (curTextScale >= 1 || Size.Y > oldSize.Y) {
				return;
			}

			isCurrentlyFittingChildren = true;

			var i = 0;
			while (curTextScale < 1 && i < 99) {
				var oldTextScale = curTextScale;

				curTextScale += 0.05f;

				ResizeAllTexts();
				await ResetHeight();

				await ToSignal(this, "sort_children");

				if (Size.Y > targetSize.Y) {
					curTextScale = oldTextScale;
					ResizeAllTexts();
					await ResetHeight();

					break;
				}

				i++;
			}

			isCurrentlyFittingChildren = false;
			return;
		}

		isCurrentlyFittingChildren = true;

		var j = 0;
		while (Size.Y > targetSize.Y && j < 99) {
			curTextScale -= 0.05f;

			ResizeAllTexts();
			await ResetHeight();

			j++;
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

	void ResizeAllTexts() {
		var texts = GetTextChildren();
		foreach (var text in texts) {
			text.SetSystemScale(curTextScale);
		}
	}
}
