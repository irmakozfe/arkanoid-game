using System;
using GArkanoid.Entities;
using Godot;

namespace Garkanoid.PowerUps
{

    //represents the powerup node before collecting and activating it
    // it should be spawned randomly when a block is destroyed
    public partial class PowerUp : Area2D
    {
        [Signal] public delegate void PowerUpCollectedEventHandler(PowerUp powerUp);
        [Signal] public delegate void PowerUpExpiredEventHandler(PowerUp powerUp);

        [Export] private float _fallSpeed = 100f;
        [Export] private float _despawnTime = 5f;

        [Export] public PowerUpType PowerUpType
        {
            get;
            set;
        } = PowerUpType.None;

        private float _aliveTime = 0f; 
        private bool _isCollected = false;

        public override void _EnterTree()
        {
            BodyEntered += OnBodyEntered;
        }

        public override void _ExitTree()
        {
            BodyEntered -= OnBodyEntered;
        }

        public override void _PhysicsProcess(double delta)
        {
            float deltaTime = (float)delta;

            if (_isCollected) return;

            GlobalPosition += Vector2.Down * _fallSpeed * deltaTime;

            _aliveTime += deltaTime;

            //Despawn the powerup node if its not collected on time 
            if (_aliveTime >= _despawnTime)
            {
                EmitSignal(SignalName.PowerUpExpired, this);
                QueueFree();
            }


        }

        private void OnBodyEntered(Node2D body)
        {
            // Only the paddle should be able to collect the powerup

            if(!_isCollected && body is Paddle paddle)
            {
                CollectPowerUp();
            }
        }

        private void CollectPowerUp()
        {
            if (_isCollected)
            {
                return;
            }

            _isCollected = true;

            EmitSignal(SignalName.PowerUpCollected, this);

            QueueFree();
        }
    } 
    
}