using Godot;
using Godot.Collections;
using System;

public partial class IconMenu : SubTypeMenu
{
    public MoveableArt linkedArt;

    // --- SAVE HANDLING ---
	public override Dictionary Save() {
		var dict = base.Save();

		dict.Add("LinkedArt", linkedArt.Save());

		return dict;
	}

	public override void Load(Dictionary data) {
        base.Load(data);

        var linkedArtProps = (Dictionary) data["LinkedArt"];
		linkedArt.Load(linkedArtProps);
	}
}
