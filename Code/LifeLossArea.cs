using GArkanoid.Entities;
using GArkanoid.Systems;
using Godot;
using System;

namespace Garkanoid.Entities{
public partial class LifeLossArea : StaticBody2D
{
	public void OnBallHit(Ball ball)
		{
			bool hasLivesLeft = GameManager.Instance.RemoveLife();

			if (hasLivesLeft)
			{
				ball.ResetBall();
			} else
			{
				ball.QueueFree();
			}
		}
}
}
