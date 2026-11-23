using Godot;
using System;
using System.Security.AccessControl;
using Garkanoid.Save;
using Godot.Collections;

namespace GArkanoid.Entities
{
	[Tool]
 	public partial class Paddle : CharacterBody2D, ISaveable
	{
		public enum ControlMode
		{
			Keyboard,
			Mouse
		}
		[Export] private float _speed = 300;
		[Export] private ControlMode _controlMode =ControlMode.Keyboard;

		private Ball _ball;

		[Export] public string ID {get; set;} = System.Guid.NewGuid().ToString();




		public override void _PhysicsProcess(double delta)
		{
			float deltaTime = (float)delta;

			if (Engine.IsEditorHint()) return;

			if (Input.IsActionJustPressed(Config.ClickToggleActionName)) 
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
				float horizontal = Input.GetAxis(Config.axisNameLeft, Config.axisNameRight);
				moveX = horizontal * _speed * deltaTime;
				break;
			}

			MoveAndCollide(new Vector2(moveX, 0));
		}

        public bool Save(ref Dictionary<string, Variant> saveData)
        {
			if (saveData == null) return false;

			saveData[ID] = new Dictionary<string, Variant>
			{
				{Config.PositionXKey, GlobalPosition.X},
				{Config.ControlModeKey, (int) _controlMode}	
			};

			return true;
        }

        public bool Load(Dictionary<string, Variant> saveData)
        {
			if (saveData == null || !saveData.TryGetValue(ID, out Variant value))
			{
				GD.PushError($"No save data for paddle '{ID}'.");
				return false;
			}

			var data = value.AsGodotDictionary<string, Variant>();
			GlobalPosition = new Vector2(data[Config.PositionXKey].AsSingle(), GlobalPosition.Y);
			_controlMode = (ControlMode)data[Config.ControlModeKey].AsInt32();
			return true;         
        }
    }

}
