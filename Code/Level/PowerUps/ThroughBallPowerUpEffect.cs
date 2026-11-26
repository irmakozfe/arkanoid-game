using GArkanoid.Entities;

namespace Garkanoid.PowerUps
{
    public partial class ThroughBallPowerUpEffect : PowerUpEffect
    {
        public override PowerUpType PowerUpType => PowerUpType.ThroughBall;

        public override void Activate()
        {
            base.Activate();
            Level.Current.Ball.isThroughBallActive = true;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            Level.Current.Ball.isThroughBallActive = false;
        }
    }
}