using GArkanoid.Systems;
using Godot;
using System;

public partial class Block : StaticBody2D
{
	[Signal] public delegate void BlockDestroyedEventHandler(Block block);
	[Export] private int _score = 10;

	public void Hit()
	{
		// Gets should be called whenever a ball hits the block
		// every block gets destroyed by hitting them once 
		// TODO: implement health functionality to enable multiple hit support 
		GameManager.Instance.AddScore(_score);
		EmitSignal(SignalName.BlockDestroyed, this);
		QueueFree(); // destroys the block 
		// Free() destroys the node immediately 
		
	}

}
