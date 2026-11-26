namespace Garkanoid.PowerUps
{
    public partial class ExplodingBallPowerUpEffect : PowerUpEffect
    {
        public override PowerUpType PowerUpType => PowerUpType.ExplodingBall;

        ExplodingBallPowerUpEffect()
        {
            Duration = 5f;
        }

        public override void Activate()
        {
            base.Activate();
        }


        public override void Deactivate()
        {
            base.Deactivate();
        }


    }
}