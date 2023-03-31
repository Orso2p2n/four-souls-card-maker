using Godot;
using System;

public partial class MoveableArtBase : TextureRect
{
	public async void SetTexture(Texture2D texture) {
		Texture = texture;
		
		await ToSignal(RenderingServer.Singleton, "frame_post_draw");

		PostSetTexture();
	}
	
	public virtual void PostSetTexture() {		
		var halfSize = GetRect().Size / 2;
		PivotOffset = halfSize;
	}
}
