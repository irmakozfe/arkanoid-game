using Godot;
using System;

namespace GArkanoid.Entities
{
	public partial class Ball : CharacterBody2D
	{
		
		[Export] private float _speed = 300;
		[Export] private Vector2 _direction = new Vector2(1,-1);
		[Export] private Paddle _paddle;
		[Export] private Vector2 _offset = new Vector2(0, -42); // offset -> tried some numbers for the positioning 

		private bool _isLaunched = false; 
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

			if (!_isLaunched)
			{
				FollowPaddle();
				return;
			}

			Move(deltaTime);
		}

		private void FollowPaddle()
		{
			GlobalPosition = _paddle.GlobalPosition + _offset;

			if (Input.IsActionJustPressed("Launch"))
			{
				_isLaunched = true;
			}
		}

		private void Move(float delta)
		{
			KinematicCollision2D collision = MoveAndCollide(_direction * _speed * delta);

			if(collision != null)
			{
				GodotObject collider = collision.GetCollider();
				if (collider != null)
				{
					if (collider is Block block)
					{
						block.Hit();
					}
				}
				_direction = _direction.Bounce(collision.GetNormal()).Normalized();
				Velocity = _direction * _speed;
			}
		}
}
}
