using GArkanoid.Systems;

namespace Garkanoid.PowerUps
{
    public partial class LoseLifePowerUpEffect : PowerUpEffect
    {
        public override PowerUpType PowerUpType => PowerUpType.LoseLife;

        public override void Activate()
        {
            base.Activate();
            GameManager.Instance.RemoveLife();
            
            // This powerup is not timed so it can be deactivated immediately.
            Deactivate();
        }
    }
}