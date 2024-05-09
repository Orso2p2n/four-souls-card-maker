using Godot;
using System;

public partial class CardPreview : SubViewportContainer
{
	[Export] SubViewport viewport;
    Vector2 baseWindowSize;
    float baseScale;

    public override void _EnterTree() {
        float width = (float) Godot.ProjectSettings.GetSetting("display/window/size/viewport_width");
        float height = (float) Godot.ProjectSettings.GetSetting("display/window/size/viewport_height");
        baseWindowSize = new Vector2(width, height);

        GetTree().Root.SizeChanged += OnResize;
    }

    public override void _Ready() {
        base._Ready();

		viewport.HandleInputLocally = true;
        baseScale = Scale.X;
    }

    void OnResize() {
        var newWindowSize =  GetTree().Root.Size;

        var widthPercent = newWindowSize.X / baseWindowSize.X;
        var heightPercent = newWindowSize.Y / baseWindowSize.Y;

        var minPercent = Mathf.Min(widthPercent, heightPercent);

        var newScale = minPercent * baseScale;

        Scale = new Vector2(newScale, newScale);
    }
}
