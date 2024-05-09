using Godot;
using System;

public partial class FileDialogHandler : FileDialog
{
	Callable fileCallback;
	Callable folderCallback;

	FileSelectedEventHandler connectedFileEvent;
	DirSelectedEventHandler connectedDirEvent;

	void OnVisibilityChanged() {
		if (!Visible) {
			DisconnectAllEvents();
		}
	}

	public void LoadTextureFileDialog(Callable callback) {
        fileCallback = callback;

		FileMode = FileDialog.FileModeEnum.OpenFile;
		Filters = new string[]{"*.png, *.jpg, *.jpeg ; Supported Images"};
		Visible = true;
		ConnectToFileEvent(OnTexturePathSelected);
	}

    void OnTexturePathSelected(string path) {
		var texture = LoadTextureFromPath(path);
        
        fileCallback.Call(path, texture);
    }

	public Texture2D LoadTextureFromPath(string path) {
		var image = new Image();
        var error = image.Load(path);

        if (error != Error.Ok) {
            GD.PrintErr("Failed to load " + path + ". Error: " + error);
        }

        var texture = ImageTexture.CreateFromImage(image);
		return texture;
	}

	public void WriteSaveFileDialog(Callable callback) {
        fileCallback = callback;

		FileMode = FileDialog.FileModeEnum.SaveFile;
		Filters = new string[]{"*.fscard ; Four Souls Card"};
		Visible = true;
		ConnectToFileEvent(OnFileSelected);
	}

	public void LoadSaveFileDialog(Callable callback) {
        fileCallback = callback;

		FileMode = FileDialog.FileModeEnum.OpenFile;
		Filters = new string[]{"*.fscard ; Four Souls Card"};
		Visible = true;
		ConnectToFileEvent(OnFileSelected);
	}

	public void SelectFileOrFolderDialog(Callable _fileCallback, Callable _folderCallback) {
        fileCallback = _fileCallback;
        folderCallback = _folderCallback;

		FileMode = FileDialog.FileModeEnum.OpenAny;
		Filters = new string[]{"Folder","*.fscard ; Four Souls Card"};
		Visible = true;
		ConnectToFileEvent(OnFileSelected);
		ConnectToDirEvent(OnDirSelected);
	}

	public void SelectFolderDialog(Callable callback) {
        folderCallback = callback;

		FileMode = FileDialog.FileModeEnum.OpenDir;
		Filters = new string[]{"Folder"};
		Visible = true;
		ConnectToDirEvent(OnDirSelected);
	}

	void OnFileSelected(string path) {
		fileCallback.Call(path);	
	}

	void OnDirSelected(string path) {
		folderCallback.Call(path);
	}

	void ConnectToFileEvent(FileSelectedEventHandler toConnect) {
		connectedFileEvent = toConnect;
		FileSelected += toConnect;
	}

	void ConnectToDirEvent(DirSelectedEventHandler toConnect) {
		connectedDirEvent = toConnect;
		DirSelected += toConnect;
	}

	void DisconnectAllEvents() {
		DisconnectFileEvent();
		DisconnectDirEvent();
	}

	void DisconnectFileEvent() {
		if (connectedFileEvent != null) {
			FileSelected -= connectedFileEvent;
			connectedFileEvent = null;
		}
	}

	void DisconnectDirEvent() {
		if (connectedDirEvent != null) {
			DirSelected -= connectedDirEvent;
			connectedDirEvent = null;
		}
	}
}
