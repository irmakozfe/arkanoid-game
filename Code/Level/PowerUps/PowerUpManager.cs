using Godot;
using System;
using System.Collections.Generic;

namespace Garkanoid.PowerUps{
public partial class PowerUpManager : Node2D
	{
		[Signal] public delegate void PowerUpActivatedEventHandler(PowerUpType powerUpType);
		[Signal] public delegate void PowerUpDeactivatedEventHandler(PowerUpType powerUpType);

		[Export] private PowerUpSceneMapping[] _powerUpSceneMapping = null;
		[Export] private PowerUpEffect[] _powerUpEffects = null;

 		private List<PowerUpEffect> _activePowerUps = new List<PowerUpEffect>();

		public override void _Process(double delta)
		{
			for (int i = _activePowerUps.Count -1 ; i>= 0 ; i--)
			{
				PowerUpEffect powerUpEffect = _activePowerUps[i];
				if (powerUpEffect.Update((float)delta))
				{
					_activePowerUps.RemoveAt(i);
					EmitSignal(SignalName.PowerUpDeactivated, (int)powerUpEffect.PowerUpType);
				}
			}
		}

		public void ActivatePowerUp(PowerUpType type)
		{
			
		}

		public void DeactivatePowerUp(PowerUpType type)
		{
			PowerUpEffect effect = GetActiveEffect(type);
			if(effect != null)
			{
				effect.Deactivate();
				_activePowerUps.Remove(effect);
				EmitSignal(SignalName.PowerUpDeactivated, (int) type);
			}
			
		}

		public void DeactivateAllPowerUps()
		{
		
			
		}

		public bool IsPowerUpActive(PowerUpType type)
		{
			throw new NotImplementedException();
		}

		public PowerUpEffect GetActiveEffect(PowerUpType type)
		{
			foreach(PowerUpEffect effect in _activePowerUps)
			{
				if(effect.PowerUpType == type)
				{
					return effect;
				}
			}

			return null;
		}

		public void SpawnPowerUp(Vector2 globalPosition, PowerUpType powerUpToSpawn)
		{
			PowerUp powerUp = null;
		}

		private void HandleStackingRules(PowerUpEffect existingEffect,PowerUpType type)
		{
	
		}

		private PowerUpEffect CreatePowerUpEffect(PowerUpType type)
		{
			foreach (PowerUpEffect effectTemplate in _powerUpEffects)
			{
				if(effectTemplate.PowerUpType == type)
				{
					return effectTemplate.Duplicate() as PowerUpEffect;
				}
			}
			return null;
		}

		private void OnPowerUpCollected(PowerUp powerUp)
		{
			
		}
		private void OnPowerUpExpired(PowerUp powerUp)
		{
			
		}

		



	}
}
