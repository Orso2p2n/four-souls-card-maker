using Godot;
using System;

public partial class FileDialogHandler : FileDialog
{
	Callable fileCallback;
	Callable folderCallback;

	FileSelectedEventHandler connectedFileEvent;
	DirSelectedEventHandler connectedDirEvent;

	void OnCanceled() {
		DisconnectAllEvents();
	}

	public void LoadTextureFileDialog(Callable callback) {
        fileCallback = callback;

		FileMode = FileDialog.FileModeEnum.OpenFile;
		Filters = new string[]{"*.png, *.jpg, *.jpeg ; Supported Images"};
		ConnectToFileEvent(OnTexturePathSelected);
		Visible = true;
	}

    void OnTexturePathSelected(string path) {
		var texture = LoadTextureFromPath(path);
        
        fileCallback.Call(path, texture);
		DisconnectAllEvents();
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
		ConnectToFileEvent(OnFileSelected);
		Visible = true;
	}

	public void LoadSaveFileDialog(Callable callback) {
        fileCallback = callback;

		FileMode = FileDialog.FileModeEnum.OpenFile;
		Filters = new string[]{"*.fscard ; Four Souls Card"};
		ConnectToFileEvent(OnFileSelected);
		Visible = true;
	}

	public void SelectFileOrFolderDialog(Callable _fileCallback, Callable _folderCallback) {
        fileCallback = _fileCallback;
        folderCallback = _folderCallback;

		FileMode = FileDialog.FileModeEnum.OpenAny;
		Filters = new string[]{"Folder","*.fscard ; Four Souls Card"};
		ConnectToFileEvent(OnFileSelected);
		ConnectToDirEvent(OnDirSelected);
		Visible = true;
	}

	public void SelectFolderDialog(Callable callback) {
        folderCallback = callback;

		FileMode = FileDialog.FileModeEnum.OpenDir;
		Filters = new string[]{"Folder"};
		ConnectToDirEvent(OnDirSelected);
		Visible = true;
	}

	void OnFileSelected(string path) {
		fileCallback.Call(path);
		DisconnectAllEvents();
	}

	void OnDirSelected(string path) {
		folderCallback.Call(path);
		DisconnectAllEvents();
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
