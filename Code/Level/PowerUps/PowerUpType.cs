using Godot;
using System;

namespace Garkanoid.PowerUps{
public enum PowerUpType
	{
		None = 0,
		ExtraLife,
		ExpandPaddle,
		ThroughBall,
		//TODO: add 3 more powerups here 
		BallSpeedIncrease,
		LoseLife,
		ExplodingBall
	}
}
