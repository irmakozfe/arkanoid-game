
using GArkanoid.Systems;
using Godot;
using System;

namespace Garkanoid.UI{
public partial class GameOverControl : Control
	{
		[Export] private Button _retryButton = null;
		[Export] private Button _backToMenuButton = null;

		public override void _EnterTree()
		{
			_retryButton.Pressed += OnRetryPressed;
			_backToMenuButton.Pressed += OnBackToMenuPressed;
		}

		public override void _ExitTree()
		{
			_retryButton.Pressed -= OnRetryPressed;
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
