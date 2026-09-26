using Godot;
using System;
using System.Security.AccessControl;

namespace GArkanoid.Entities
{
 	public partial class Paddle : CharacterBody2D
	{
		public enum ControlMode
		{
			Keyboard,
			Mouse
		}
		[Export] private float _speed = 300;
		[Export] private ControlMode _controlMode =ControlMode.Keyboard;

		private Ball _ball;




		public override void _PhysicsProcess(double delta)
		{
			float deltaTime = (float)delta;

			if (Input.IsActionJustPressed("ClickToggle")) 
			{
				_controlMode = _controlMode == ControlMode.Keyboard 
				? ControlMode.Mouse
				: ControlMode.Keyboard;
			}

			float moveX = 0;
			switch (_controlMode)
			{
				case ControlMode.Mouse:
				moveX= GetGlobalMousePosition().X - GlobalPosition.X;
				break;

				case ControlMode.Keyboard:
				float horizontal = Input.GetAxis("Left", "Right");
				moveX = horizontal * _speed * deltaTime;
				break;
			}

			MoveAndCollide(new Vector2(moveX, 0));


			if (Input.IsActionJustPressed("Launch"))
			{
				GD.Print("Ball Launched.");
			}
		}
	}
}
