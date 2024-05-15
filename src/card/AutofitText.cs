using Godot;
using System;
using System.Threading.Tasks;

public partial class AutofitText : RichTextLabel
{
	[Export] Container parentContainer;

	FontVariation fontVar;

	float targetWidth;

	int baseSpacingGlyph;
	int baseSpacingSpace;
	Vector2 baseTransformX;

	int baseFontSize;

	int curSpacingGlyph;

	[Export] int maxSpacingGlyph = -3;
	[Export] int maxSpacingSpace = -10;

    public override void _Ready() {
		var font = GetThemeFont("normal_font").Duplicate();
		fontVar = (FontVariation) font;

		targetWidth = parentContainer.Size.X;

		curSpacingGlyph = baseSpacingGlyph = fontVar.SpacingGlyph;
		baseSpacingSpace = fontVar.SpacingSpace;
		baseTransformX = fontVar.VariationTransform.X;

		baseFontSize = GetThemeFontSize("normal_font_size");

		AddThemeFontOverride("normal_font", fontVar);
    }

    public void SetText(string text) {
		Text = text;
		_ = Autofit();
	}

	async Task Autofit() {
		Card.instance.PauseRender((int) GetInstanceId());

		ResetFont();

		await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);

		var step = 0;

		while (targetWidth < Size.X) {
			var success = false;

			switch (step) {
				case 0:
					success = TryReduceGlyphSpacing();
					break;
				
				case 1:
					success = TryReduceSpaceSpacing();
					break;

				case 2:
					success = TryReduceScaleX();
					break;
			}

			if (success) {
				await ToSignal(GetTree(), SceneTree.SignalName.ProcessFrame);
			}

			step++;

			if (step > 2) {
				step = 0;
			}
		}

		Card.instance.ResumeRender((int) GetInstanceId());
	}

	bool TryReduceGlyphSpacing() {
		if (curSpacingGlyph > maxSpacingGlyph) {
			fontVar.SpacingGlyph--;
			curSpacingGlyph--;
			return true;
		}

		return false;
	}

	bool TryReduceSpaceSpacing() {
		if (fontVar.SpacingSpace > maxSpacingSpace) {
			fontVar.SpacingSpace--;
			return true;
		}

		return false;
	}

	bool TryReduceScaleX() {
		var reduceBy = 1f / baseFontSize;

		var X = fontVar.VariationTransform.X.X;
		var factorX = (X - reduceBy) / X; 
		
		fontVar.VariationTransform = fontVar.VariationTransform.Scaled(new Vector2(factorX, 1));

		fontVar.SpacingGlyph--;

		return true;
	}

	void ResetFont() {
		curSpacingGlyph = fontVar.SpacingGlyph = baseSpacingGlyph;
		fontVar.SpacingSpace = baseSpacingSpace;

		fontVar.VariationTransform = new Transform2D(baseTransformX, fontVar.VariationTransform.Y, fontVar.VariationTransform.Origin);
	}
}
