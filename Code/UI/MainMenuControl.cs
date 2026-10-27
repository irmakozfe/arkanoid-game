using GArkanoid.Systems;
using Godot;
using System;

namespace Garkanoid.UI{
public partial class MainMenuControl : Control
{
	[Export] private Button _newGameButton = null;
	[Export] private Button _settingsButton = null;
	[Export] private Button _quitButton = null;

		public override void _EnterTree()
		{
			_newGameButton.Pressed += OnNewGamePressed;
			_settingsButton.Pressed +=  OnSettingsPressed;
			_quitButton.Pressed += OnQuitPressed;
		}

		public override void _ExitTree()
		{
			_newGameButton.Pressed -= OnNewGamePressed;
			_settingsButton.Pressed -= OnSettingsPressed;
			_quitButton.Pressed -= OnQuitPressed;
		}

		private void OnNewGamePressed()
		{
			GameManager.Instance.ResetGame();
			if (!GameManager.Instance.ChangeState(GameStateType.InGame)){
				GD.PushError("Loading a new game failed");
			}

		}

		private void OnSettingsPressed()
		{
			if (!GameManager.Instance.ChangeState(GameStateType.Settings))
			{
				GD.PushError("Settings screen is not found");
			}
		}

		private void OnQuitPressed()
		{
			// TODO: which GameStateType is suitable for here?
		}

}
}
