using Godot;
using System;
using System.Threading.Tasks;

public partial class DescEffect : DescBase
{
	[Export] public RichTextLabel richText;

	int baseFontSize;
	int curFontSize;

	public float userScale = 1.0f;
	public float systemScale = 1.0f;

	public override void _Ready() {
		base._Ready();

		baseFontSize = curFontSize = richText.GetThemeFontSize("normal_font_size");
	}

	public override void _Process(double delta) {
		base._Process(delta);

        if (!container.isCurrentlyFittingChildren) {
		    UpdateSize();
        }
	}

	public void UpdateSize() {
		var height = richText.Size.Y;
		CustomMinimumSize = Size = new Vector2(0, height + padding);
		richText.Position = new Vector2(0, padding/2);
	}

	public void SetText(string text) {
		richText.Text = "[center]" + text;

		ResetRichTextSize();
	}

	public void SetUserScale(float value) {
		userScale = value;

		ResetRichTextSize();
	}

	public void SetSystemScale(float value) {
		systemScale = value;

		ResetRichTextSize();
	}

	public override void SetPadding(int value) {
		base.SetPadding(value);

		UpdateSize();
	}

	public void ResetRichTextSize() {
        curFontSize = (int) (baseFontSize * userScale * systemScale);
        richText.AddThemeFontSizeOverride("normal_font_size", curFontSize);
        richText.Size = new Vector2(richText.Size.X, 0);

        UpdateSize();
	}
}
