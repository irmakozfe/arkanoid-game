using GArkanoid.Entities;
using GArkanoid.Systems;
using Godot;
using System;

public partial class Block : StaticBody2D
{
	[Signal] public delegate void BlockDestroyedEventHandler(Block block);
	[Export] private int _score = 10;

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
		EmitSignal(SignalName.BlockDestroyed, this);
		QueueFree(); // destroys the block 
		// Free() destroys the node immediately 
		
	}

}
