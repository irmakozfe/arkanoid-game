using GArkanoid.Systems;
using Godot;
using System;

namespace Garkanoid.UI{
public partial class LevelUI : CanvasLayer
{
	[Export] private Label _scoreLabel = null;
	[Export] private Label _livesLabel = null;

		public override void _EnterTree()
		{
			GameManager.Instance.ScoreChanged += OnScoreChanged;
			GameManager.Instance.LivesChanged += OnLivesChanged;
		}

		public override void _ExitTree()
		{
			GameManager.Instance.ScoreChanged -= OnScoreChanged;
			GameManager.Instance.LivesChanged -= OnLivesChanged;
		}

		public override void _Ready()
		{
			OnScoreChanged(GameManager.Instance.Score);
			OnLivesChanged(GameManager.Instance.Lives);
		}

		private void OnScoreChanged(int score)
		{
			if (_scoreLabel != null)
			{
				_scoreLabel.Text = $"Score: {score}";
			}
		}

		private void OnLivesChanged(int lives)
		{
		
			if (_livesLabel != null)
			{
				_livesLabel.Text = $"Lives: {lives}";
			}
		}

}
}
