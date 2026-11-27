using Godot;
using System;
using GArkanoid.Common;
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
		[Export] private  Sprite2D _sprite;
		[Export] private CollisionShape2D _collisionShape;

		private Ball _ball;
		private float _widthMultiplyer = 1f; 
		private float _initialWidthScale = 1;

		public float WidthMultiplyer
		{
			get => _widthMultiplyer;
			set
			{
				_widthMultiplyer = value;
				UpdateWidth();
			}
		}

		[Export] public string ID {get; set;} = System.Guid.NewGuid().ToString();

        public override void _Ready()
        {
            base._Ready(); 

			if (_collisionShape == null)
			{
				_collisionShape = this.GetNode<CollisionShape2D>();
			}

			if(_sprite == null)
			{
				_sprite = this.GetNode<Sprite2D>();
			}

			if(_sprite != null){

			_initialWidthScale = _sprite.Scale.X;
			} 
			else
			{
				GD.PushError("Sprite for the paddle is not set!");
			}

        }

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

		public void Expand(float multiplyer)
		{
			WidthMultiplyer *= multiplyer;
		}

		public void Schrink(float multiplyer)
		{
			WidthMultiplyer *= 1 / multiplyer;
		}

		private void UpdateWidth()
		{
			// TODO: make sure the paddle fits the level
			float xScale = _initialWidthScale * WidthMultiplyer;
			float yScale =_sprite.Scale.Y;

			_sprite.Scale = new Vector2(xScale, yScale);
			_collisionShape.Scale = new Vector2(yScale, xScale);
		}
    }

}
