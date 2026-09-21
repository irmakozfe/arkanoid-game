using Godot;
using System;

namespace GArkanoid.Entities
{
 	public partial class Paddle : CharacterBody2D
	{
		[Export] private float _speed = 300;

		private Ball _ball;
		private bool _mouseControlEnabled = false;



		public override void _Process(double delta)
		{
			float deltaTime = (float)delta;

			if (Input.IsActionJustPressed("ClickToggle")) 
			{
				_mouseControlEnabled = !_mouseControlEnabled;
			}

			if (_mouseControlEnabled)
			{
				float mouse = GetGlobalMousePosition().X;
				Position = new Vector2(mouse, Position.Y);
			}
			else
			{
				float horizontal = Input.GetAxis("Left", "Right");
				Position += new Vector2(horizontal, 0) * _speed * deltaTime;
			}

			float viewportX = GetViewportRect().Size.X;
			float paddleHalfWidth= GetNode<Sprite2D>("Sprite2D").GetRect().Size.X * Scale.X / 8; // I found this scaling by trying out  
			Position = new Vector2(Mathf.Clamp(Position.X, paddleHalfWidth, viewportX - paddleHalfWidth), Position.Y);


			if (Input.IsActionJustPressed("Launch"))
			{
				GD.Print("Ball Launched.");
			}
		}
	}
}
