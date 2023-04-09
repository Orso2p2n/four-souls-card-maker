using Godot;
using Godot.Collections;
using System;

public partial class IconMenu : SubTypeMenu
{
    public MoveableArt linkedArt;

    // --- SAVE HANDLING ---
	public override Dictionary Save() {
		var dict = base.Save();

		dict.Add("X", linkedArt.Position.X);
		dict.Add("Y", linkedArt.Position.Y);
		dict.Add("Scale", linkedArt.Scale.X);

		return dict;
	}

	public override async void Load(Dictionary data) {
        base.Load(data);

        await ToSignal(RenderingServer.Singleton, "frame_post_draw");

		var x = (float) data["X"];
		var y = (float) data["Y"];
		linkedArt.SetPosition(new Vector2(x,y));

		var scale = (float) data["Scale"];
		linkedArt.SetScale(scale);
	}
}
