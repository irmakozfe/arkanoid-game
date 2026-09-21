using Godot;
using System;

namespace GArkanoid.Entities
{
	public partial class Ball : Node2D
	{
		
		[Export] private float _speed = 100;

	public float Speed
	{
		get { return _speed; }
	}
}
}
