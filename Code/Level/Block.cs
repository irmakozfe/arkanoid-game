using GArkanoid.Entities;
using GArkanoid.Systems;
using Godot;
using GArkanoid.Common;


public partial class Block : StaticBody2D
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
		_collider.CallDeferred("Disabled", false);
	}

	public void Disable()
	{
		Hide();
		if(_collider != null){
			_collider.SetDeferred("Disabled", true);
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

}
