using Garkanoid.Effects;
using Garkanoid.Entities;
using Godot;
using System;
using Garkanoid.Save;
using Godot.Collections;

namespace GArkanoid.Entities
{
	public partial class Ball : CharacterBody2D, ISaveable
	{
		
		[Export] private float _speed = 300;
		[Export] private Vector2 _direction = new Vector2(1,-1);
		[Export] private Paddle _paddle;
		[Export] private Vector2 _offset = new Vector2(0, -42); // offset -> tried some numbers for the positioning 

		private bool _isLaunched = false; 
		private Vector2 _startDirection;
		[Export] public string ID {get; set;} = Config.BallKey;

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
		 _startDirection = _direction;
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

			if (Input.IsActionJustPressed(Config.LaunchActionName))
			{
				_isLaunched = true;
			}
		}

		private void Move(float delta)
		{
			KinematicCollision2D collision = MoveAndCollide(_direction * _speed * delta);
			EffectType effect = EffectType.None;
			if(collision != null)
			{
				GodotObject collider = collision.GetCollider();
				if (collider != null)
				{
					 effect = EffectType.Bounce;
					if (collider is Block block)
					{
						effect = EffectType.Hit;
						block.Hit();
					}

					if (collider is LifeLossArea lifeLossArea)
					{
						Level.Current.EffectPlayer.PlayEffect(EffectType.Death, GlobalPosition);
						lifeLossArea.OnBallHit(this);
						return;
					}
				}

				if (effect != EffectType.None){
				Level.Current.EffectPlayer.PlayEffect(effect, GlobalPosition);
				}
				_direction = _direction.Bounce(collision.GetNormal()).Normalized();
				Velocity = _direction * _speed;
			}
		}

		public void ResetBall()
		{
			_isLaunched = false;
			_direction = _startDirection;
			Velocity = Vector2.Zero;
			GlobalPosition = _paddle.GlobalPosition + _offset;
		}

        public bool Save(ref Dictionary<string, Variant> saveData)
        {
			if (saveData == null) return false;

			saveData[ID] = new Dictionary<string, Variant>
			{
				{ Config.PositionXKey, GlobalPosition.X },
				{ Config.PositionYKey, GlobalPosition.Y },
				{ Config.DirectionXKey, _direction.X },
				{ Config.DirectionYKey, _direction.Y },
				{ Config.IsLaunchedKey, _isLaunched },
			};
			return true;
        }

        public bool Load(Dictionary<string, Variant> saveData)
        {
			if (saveData == null || !saveData.TryGetValue(ID, out Variant value))
			{
				GD.PushError($"No save data for ball '{ID}'.");
				return false;
			}

			var data = value.AsGodotDictionary<string, Variant>();
			GlobalPosition = new Vector2(data[Config.PositionXKey].AsSingle(), data[Config.PositionYKey].AsSingle());
			_direction = new Vector2(data[Config.DirectionXKey].AsSingle(), data[Config.DirectionYKey].AsSingle()).Normalized();
			_isLaunched = data[Config.IsLaunchedKey].AsBool();
			Velocity = _direction * _speed;
			return true;

        }
    }
}
