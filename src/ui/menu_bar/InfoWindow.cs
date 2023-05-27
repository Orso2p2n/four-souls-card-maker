using Godot;
using System;

public partial class InfoWindow : Window
{
	public void OnCloseRequested() {
		Visible = false;
	}

	public void OnTextMetaClicked(string url) {
		OS.ShellOpen(url);
	}
}

