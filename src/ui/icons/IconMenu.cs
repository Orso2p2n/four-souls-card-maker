using Godot;
using Godot.Collections;
using System;
using System.Threading.Tasks;

public partial class IconMenu : SubTypeMenu
{
    public MoveableArt linkedArt;

	public override void _Ready() {
		base._Ready();
	
		linkedArt.trashCallable = new Callable(this, "TrashLinkedArt");
	}

    // --- SAVE HANDLING ---
	public override Dictionary Save() {
		var dict = base.Save();

		dict.Add("LinkedArt", linkedArt.Save());

		return dict;
	}

	public override void Load(Dictionary data) {
        base.Load(data);

        var linkedArtProps = (Dictionary) data["LinkedArt"];
		
		_ = linkedArt.Load(linkedArtProps);
	}

	public void TrashLinkedArt(MoveableArt trashedArt) {
		if (trashedArt == linkedArt) {
			ResetSelection();
		}
	}
}
