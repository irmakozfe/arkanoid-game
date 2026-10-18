using Godot;
using System;

namespace GArkanoid.Systems{
public partial class GameManager : Node
{
	#region Singleton
	public static GameManager Instance
		{
			get;
			private set;
		}

		//singleton functionality: 
	public sealed override void _Ready()  // sealed: no further overrides are possible
		{
			GD.Print("Initializing GameManager...");
			if(Instance == null)
			{
				Instance = this;
			} else if (Instance != this) {
				// the one and only instance class already exists.
				QueueFree(); // delete it safely 
				return; // makes sure that nothing happens after queuefree
			}
			Initialize();
			
		}
		#endregion

		// naming is important ....EventHandler();
		[Signal]
		public delegate void ScoreChangedEventHandler(int score);
		
		[Signal]
		public delegate void LivesChangedEventHandler(int lives); 
		
		private int _score = 0;
		private int _lives = 3;

		public int Lives
		{
			get{return _lives;}
			private set
			{
				_lives = Mathf.Max(value, 0);
				GD.Print($"Lives: {_lives}");
				EmitSignal(SignalName.LivesChanged, _lives);
			}
		}
		public int Score
		{
			get{return _score;}
			private set
			{
				// TODO validate the score value 
				_score = value;
				EmitSignal(SignalName.ScoreChanged, _score);
				// TODO notify about updating the score!
			}
		}

		protected virtual void Initialize()
		{
			GD.Print("GameManager is initialized.");
		}

		public void AddScore(int scoreToAdd)
		{
			if (scoreToAdd <= 0)
			{
				return;
			}
			
			Score += scoreToAdd;

			
		}

		public bool RemoveLife()
		{

			if (Lives <= 0) return false;

			Lives--;

			if(Lives == 0)
			{
				GD.Print("Game Over!");
				return false;
			}

			return true;
		}

}
}
