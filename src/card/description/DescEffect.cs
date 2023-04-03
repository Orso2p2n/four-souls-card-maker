using Godot;
using System;
using System.Threading.Tasks;

public partial class DescEffect : DescBase
{
	[Export] public RichTextLabel richText;

	int baseFontSize;
	public float userScale = 1.0f;
	public float systemScale = 1.0f;

	float baseWidth;
	public float boundsMul = 1.0f;

	int baseLineSpacing;
	public int lineSpacingDelta;

	FontVariation fontVar;
	public int characterSpacing;

	public override void _Ready() {
		base._Ready();

		baseFontSize = richText.GetThemeFontSize("normal_font_size");
		baseLineSpacing = richText.GetThemeConstant("line_separation");
		baseWidth = richText.Size.X;

		var font = richText.GetThemeFont("normal_font").Duplicate();
		fontVar = (FontVariation) font;
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
		richText.Position = new Vector2(container.Size.X / 2 - richText.Size.X / 2, padding/2);
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

	public void SetBoundsMul(float value) {
		boundsMul = value;

		ResetRichTextSize();
	}

	public void SetLineSpacing(int value) {
		lineSpacingDelta = value;

		ResetRichTextSize();
	}

	public void SetCharacterSpacing(int value) {
		characterSpacing = value;

		ResetRichTextSize();
	}

	public void ResetRichTextSize() {
        var curFontSize = (int) (baseFontSize * userScale * systemScale);
        richText.AddThemeFontSizeOverride("normal_font_size", curFontSize);

		var curLineSpacing = baseLineSpacing + lineSpacingDelta;
        richText.AddThemeConstantOverride("line_separation", curLineSpacing);

		fontVar.SpacingGlyph = characterSpacing;
        richText.AddThemeFontOverride("normal_font", fontVar);

        richText.Size = new Vector2(baseWidth * boundsMul, 0);

        UpdateSize();
	}
}
