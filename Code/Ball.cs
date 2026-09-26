using Godot;
using System;

namespace GArkanoid.Entities
{
	public partial class Ball : CharacterBody2D
	{
		
		[Export] private float _speed = 200;
		[Export] private Vector2 _direction = new Vector2(1,-1);

	public float Speed
	{
		get { return _speed; }
	}

	public Vector2 Direction
		{
			get {return _direction;}
		}

		public override void _Ready()
		{
		 _direction = _direction.Normalized();	
		}

		public override void _PhysicsProcess(double delta)
		{
			float deltaTime = (float)delta;

			Vector2 ballMovement  = _direction * _speed * deltaTime;
			KinematicCollision2D collision = MoveAndCollide(ballMovement);

			if (collision != null)
			{
				Vector2 normal = collision.GetNormal();
				_direction = _direction -2 * _direction.Dot(normal)* normal;
			}
		}
}
}
