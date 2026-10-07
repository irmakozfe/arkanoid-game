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
		
		private int _score = 0;

		public int Score
		{
			get{return _score;}
			set
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

}
}
