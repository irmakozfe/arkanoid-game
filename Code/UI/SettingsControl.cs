using GArkanoid.Systems;
using Godot;
using System;
using System.Reflection.PortableExecutable;

namespace Garkanoid.UI{
public partial class SettingsControl : Control
	{
		[Export] private Button _closeButton = null;

		public override void _EnterTree()
		{
			_closeButton.Pressed += OnClosePressed;
		}

		public override void _ExitTree()
		{
			_closeButton.Pressed -= OnClosePressed;
		}

		private void OnClosePressed()
		{
			if (!GameManager.Instance.ActivatePreviousState())
			{
				GD.PushError("Game can not be continiued");
			}
			
		}

	}
}
