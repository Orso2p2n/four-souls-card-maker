using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class IconMenu : SubTypeMenu
{
    public MoveableArt linkedArt;

    // --- SAVE HANDLING ---
	public override Dictionary Save() {
		var dict = base.Save();

		dict.Add("LinkedArt", linkedArt.Save());

		return dict;
	}

	public async override Task Load(Dictionary data) {
        await base.Load(data);

        var linkedArtProps = (Dictionary) data["LinkedArt"];
		
		await linkedArt.Load(linkedArtProps);
	}
}
