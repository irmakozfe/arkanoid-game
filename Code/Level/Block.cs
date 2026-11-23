using GArkanoid.Entities;
using GArkanoid.Systems;
using Godot;
using Godot.Collections;
using GArkanoid.Common;
using Garkanoid.Save;

[Tool]
public partial class Block : StaticBody2D, ISaveable
{
	[Signal] public delegate void BlockDestroyedEventHandler(Block block);
	[Export] private int _score = 10;

	private bool _isDestroyed = false;

	private CollisionShape2D _collider = null;

	public bool IsDestroyed
	{
		get => _isDestroyed;
		set
		{
			_isDestroyed =value;
			if (_isDestroyed)
			{
				EmitSignal(SignalName.BlockDestroyed, this);
				Disable();
			}
			else
			{
				Enable();
			}

		}
	}
	[Export] public string ID {get; set;} = System.Guid.NewGuid().ToString();
	public override void _Ready()
	{
		_collider = this.GetNode<CollisionShape2D>(recursive: true);
	}

	public void Enable()
	{
		Show();
		_collider.CallDeferred(CollisionShape2D.PropertyName.Disabled, false);
	}

	public void Disable()
	{
		Hide();
		if(_collider != null){
			_collider.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
		}
	}



	public override void _EnterTree()
	{
		AddToGroup(Config.BlockGroup);
	}

	public void Hit()
	{
		// Gets should be called whenever a ball hits the block
		// every block gets destroyed by hitting them once 
		if (IsQueuedForDeletion()) return;

		GameManager.Instance.AddScore(_score);
		RemoveFromGroup(Config.BlockGroup);
		IsDestroyed = true;
		
	}

	public bool Save(ref Dictionary<string, Variant> saveData)
	{
		//TODO:: 
		if(saveData == null)
		{
			GD.PushWarning("Save Data was null. This may be a bug!");
			saveData = new Dictionary<string, Variant>();
		}

		var blockData = new Dictionary<string, Variant>();
		blockData["IsDestroyed"] = IsDestroyed;

		saveData[ID] = blockData;
		
		return true;
	}

	public bool Load(Dictionary<string, Variant> saveData)
	{
		if (saveData == null || !saveData.ContainsKey(ID))
		{
			GD.PushError("");
			return false;
		}

		var blockData = (Dictionary<string, Variant>) saveData[ID];

		if(blockData == null)
		{
			GD.PushError("");
			return false;
		}

		if (blockData.ContainsKey("IsDestroyed"))
		{
			IsDestroyed = (bool) blockData["IsDestroyed"];
		}

		return true;
	}
}
