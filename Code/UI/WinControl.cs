using GArkanoid.Systems;
using Godot;
using System;

namespace Garkanoid.UI
{
	public partial class WinControl : Control
	{
		[Export] private Button _restartButton;
		[Export] private Button _backToMenuButton;

		public override void _EnterTree()
		{
			_restartButton.Pressed += OnRetryPressed;
			_backToMenuButton.Pressed += OnBackToMenuPressed;
		}

		public override void _ExitTree()
		{
			_restartButton.Pressed -= OnRetryPressed;
			_backToMenuButton.Pressed -= OnBackToMenuPressed;
		}


		private void OnRetryPressed()
		{
			GameManager.Instance.ResetGame();
			if (!GameManager.Instance.ChangeState(GameStateType.InGame))
			{
				GD.PushError("Restarting the game failed");
			}
		}

		private void OnBackToMenuPressed()
		{
			if (!GameManager.Instance.ChangeState(GameStateType.MainMenu))
			{
				GD.PushError("Returning to main menu failed");
			}
		}


	}
}
