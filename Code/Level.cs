using Garkanoid.Effects;
using Garkanoid.Save;
using GArkanoid.Common;
using GArkanoid.Systems;
using Godot;
using System.Collections.Generic;

namespace GArkanoid.Entities{
public partial class Level : Node2D, ISaveable
{
	#region Statics
	public static Level Current { get; private set; }
	#endregion Statics

	#region Level Content
	[Export] private PackedScene[] _levelContents;
	[Export] private int _levelId = 1;
	#endregion Level Content

	[Export] private Paddle _paddle = null;
	[Export] private Ball _ball = null;
	[Export] private int _wallWidth = 0;

	private Dictionary<string, Block> _blocks = new Dictionary<string, Block>();
	public Paddle Paddle { get { return _paddle; } }
	public Ball Ball { get { return _ball; } }
	public int WallWidth { get { return _wallWidth; } }
	public EffectPlayer EffectPlayer
		{
			get;
			private set;
		}

	[Export] public string ID {get; set;} = new System.Guid().ToString();	

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

		EffectPlayer = this.GetNode<EffectPlayer>();
		if(EffectPlayer == null)
			{
				GD.PushError("No effect player found from the scene!");
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

			_blocks.Clear();

			foreach (Block block in content.GetNodes<Block>(recursive: true))
			{
				//TODO: make sure all signals are disconnected before loading 
				block.BlockDestroyed += OnBlockDestroyed;

				if (!string.IsNullOrEmpty(block.ID))
				{
					if (_blocks.ContainsKey(block.ID))
					{
						 GD.PushError("Duplicate block id found");
					}
					else
					{
						_blocks[ID] = block; 
					}
				}
				else
				{
					GD.PushError("Block does not have an ID assigned.");
				}
			}

		}

		private void OnBlockDestroyed(Block block)
		{
			if (GetTree().GetNodesInGroup(Config.BlockGroup).Count == 0)
			{
				Callable.From(() => GameManager.Instance.ChangeState(GameStateType.Win)).CallDeferred();
			}
		}

        public bool Save(ref Godot.Collections.Dictionary<string, Variant> saveData)
		{
			if(saveData == null)
			{
				GD.PushWarning("Save Data was null. This may be a bug!");
				saveData = new Godot.Collections.Dictionary<string, Variant>();
			}

			bool success = true;

			var levelData = new Godot.Collections.Dictionary<string, Variant>();


			// TODO: save the level index
			
			//save all blocks
			foreach (KeyValuePair<string, Block> blockEntry in _blocks)
			{
				Block block = blockEntry.Value;
				if (!block.Save(ref levelData))
				{
					GD.PushError("Failed to save data for block");
					success = false;
				}
			}


			// TODO: save the ball 
			// TODO: save the paddle

			saveData["Level"] = levelData;
			return success;
		}

        public bool Load(Godot.Collections.Dictionary<string, Variant> saveData)
        {
			bool success = true;

			if(Save == null || !saveData.ContainsKey("Level")){}

			var levelData = (Godot.Collections.Dictionary<string, Variant>)saveData["Level"];

			foreach(var blockEntry in _blocks)
			{
				Block block = blockEntry.Value;

				if (!block.Load(levelData))
				{
					success = false;
					
				}

				//TODO: load ball and paddle
				return success;
			}
        }
    }
}
