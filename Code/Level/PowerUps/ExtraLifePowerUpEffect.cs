using GArkanoid.Systems;

namespace Garkanoid.PowerUps
{
    public partial class ExtraLifePowerUpEffect : PowerUpEffect
    {
        public override PowerUpType PowerUpType => PowerUpType.ExtraLife;

        public override void Activate()
        {
            base.Activate();
            GameManager.Instance.AddLife();
            
            // This powerup is not timed so it can be deactivated immediately.
            Deactivate();
        }
    }
}