using GArkanoid.Entities;

namespace Garkanoid.PowerUps
{
	public partial class ThroughBallPowerUpEffect : PowerUpEffect
	{
		public override PowerUpType PowerUpType => PowerUpType.ThroughBall;

		public override void Activate()
		{
			base.Activate();
			Level.Current.Ball.IsThroughBallActive = true;
		}

		public override void Deactivate()
		{
			base.Deactivate();
			Level.Current.Ball.IsThroughBallActive = false;
		}
	}
}
