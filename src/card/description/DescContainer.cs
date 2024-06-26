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

	public Array<DescEffect> texts = new();

	public float topOffset;
	public float botOffset;

	float curTextScale = 1.0f;

	bool isProcessingTextsRescale = false;
	bool interruptTextsRescale = false;

	public Card card;

	public override void _EnterTree() {
		initialPos = Position;
		initialSize = Size;
		oldSize = Size;
	}

	public override void _Process(double delta) {
		base._Process(delta);
	}
	
	public void SetOffsets(float _topOffset, float _botOffset) {
		if (_topOffset != -1) {
			topOffset = _topOffset;
		}

		if (_botOffset != -1) {
			botOffset = _botOffset;
		}
		
		Position = targetPos = initialPos + new Vector2(0, topOffset);
		Size = targetSize = initialSize - new Vector2(0, topOffset + botOffset);

		OnAnySizeChange();
	}

	public void AddText(DescEffect descEffect) {
		descEffect.ChangeOwner(this);
		descEffect.container = this;
		descEffect.SetSystemScale(curTextScale);

		descEffect.OnAnySizeChange += OnAnySizeChange;

		texts.Add(descEffect);

		OnAnySizeChange();
	}

	public void OnTextRemoved(DescEffect descEffect) {
		texts.Remove(descEffect);
		
		OnAnySizeChange();
	}

	public void AddLine(DescLine descLine) {
		descLine.ChangeOwner(this);
		descLine.container = this;

		descLine.OnAnySizeChange += OnAnySizeChange;

		OnAnySizeChange();
	}

	public void OnLineRemoved(DescLine descLine) {
		OnAnySizeChange();
	}

	public void OnAnySizeChange() {
		_ = ProcessTextsRescale();
	}

	private async Task ProcessTextsRescale() {
		card.PauseRender((int) GetInstanceId());

		if (isProcessingTextsRescale) {
			interruptTextsRescale = true;
			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			interruptTextsRescale = false;
		}

		var childrenHeight = GetChildrenHeight();

		var i = 0;
		var j = 0;

		isProcessingTextsRescale = true;

		while (childrenHeight < targetSize.Y && curTextScale < 1 && i < 99) {
			if (interruptTextsRescale) {
				return;
			}

			curTextScale += 0.01f;

			ResizeAllTexts();

			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

			childrenHeight = GetChildrenHeight();

			i++;
		}

		while (childrenHeight > targetSize.Y && j < 99) {
			if (interruptTextsRescale) {
				return;
			}

			curTextScale -= 0.01f;

			ResizeAllTexts();

			await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

			childrenHeight = GetChildrenHeight();

			j++;
		}

		isProcessingTextsRescale = false;

		Size = targetSize;

		card.ResumeRender((int) GetInstanceId());
	}

	private float GetChildrenHeight() {
		float totalHeight = 0;
		var children = GetChildren();

		foreach (Control child in children) {
			totalHeight += child.Size.Y;
		}

		return totalHeight;
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
		foreach (var text in texts) {
			text.SetSystemScale(curTextScale);
		}
	}
}
