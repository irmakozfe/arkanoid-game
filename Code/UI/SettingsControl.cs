using GArkanoid.Systems;
using Godot;
using System;
using System.Reflection.PortableExecutable;

namespace Garkanoid.UI{
public partial class SettingsControl : Control
	{
		[Export] private Button _closeButton = null;
		[Export] private Button _saveButton = null;
		[Export] private Button _cancelButton = null;

		public override void _EnterTree()
		{
			_closeButton.Pressed += OnClosePressed;
			_saveButton.Pressed += OnSaveButton;
			_cancelButton.Pressed += OnCancelButton;
		}

		public override void _ExitTree()
		{
			_closeButton.Pressed -= OnClosePressed;
			_saveButton.Pressed -= OnSaveButton;
			_cancelButton.Pressed -= OnCancelButton;
		}

		private void OnClosePressed()
		{
			if (!GameManager.Instance.ActivatePreviousState())
			{
				GD.PushError("Game can not be continiued");
			}
			
		}

		private void OnSaveButton()
		{
			//TODO: implement saving Settings
			GD.Print("Saving the settings...");
		}

		private void OnCancelButton(){}

	}
}
