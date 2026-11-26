using GArkanoid.Entities;

namespace Garkanoid.PowerUps
{
    public partial class BallSpeedIncreasePowerUpEffect : PowerUpEffect
    {
        public override PowerUpType PowerUpType => PowerUpType.BallSpeedIncrease;

        public BallSpeedIncreasePowerUpEffect()
        {
            Duration = 5f;
        }
        public override void Activate()
        {
            base.Activate();
            Level.Current.Ball.SpeedMultiplier = 2f;
        }

        public override void Deactivate()
        {
            base.Deactivate();
            Level.Current.Ball.SpeedMultiplier = 1f;
        }
    }
}