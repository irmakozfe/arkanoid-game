using GArkanoid.Common;
using GArkanoid.Systems;
using Godot;
using System.Collections.Generic;

namespace GArkanoid.Entities{
public partial class Level : Node2D
{
	#region Statics
	public static Level Current { get; private set; }
	#endregion Statics

	#region Level Content
	[Export] private PackedScene[] _levelContents;
	[Export] private int _levelId = 1;
	#endregion Level Content

	#region References
	[Export] private Paddle _paddle = null;
	[Export] private Ball _ball = null;
	[Export] private int _wallWidth = 0;

	public Paddle Paddle { get { return _paddle; } }
	public Ball Ball { get { return _ball; } }
	public int WallWidth { get { return _wallWidth; } }
		#endregion References

		public override void _Input(InputEvent @event)
		{
			if (@event.IsActionPressed(Config.PauseActionName))
			{
				GameManager.Instance.ChangeState(GameStateType.Pause);
			}
		}

	public override void _Ready()
	// Called when the node enters the scene tree for the first time.
	{
		LoadLevelContent();
		Current = this;

		if (_paddle == null)
		{
			_paddle = this.GetNode<Paddle>();
			// same as this : _paddle = NodeExtensions.GetNode<Paddle>(this);
		}	

		if (_ball == null)
			{
				_ball = this.GetNode<Ball>();
			}

		IList<Ball> balls = this.GetNodes<Ball>(recursive:true);
			GD.Print($"Found {balls.Count} balls!");	
	}


	private void PrintAddedScore(int addedScore)
		{
			GD.Print($"Added {addedScore} score.");
		}
	
	private void LoadLevelContent()
		{
			int index = _levelId -1;
			if (_levelContents == null || index < 0 || index >= _levelContents.Length)
			{
				GD.Print($"No content for the level {_levelId}");
				return;
			}

			Node content = _levelContents[index].Instantiate();
			AddChild(content);

		}

}
}
