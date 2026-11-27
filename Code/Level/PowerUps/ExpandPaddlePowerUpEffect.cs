using GArkanoid.Entities;
using Godot;
namespace Garkanoid.PowerUps
{
    [GlobalClass]
    public partial class ExpandPaddlePowerUpEffect : PowerUpEffect
    {
        [Export] private float _multiplyer = 2f;
        public override PowerUpType PowerUpType => PowerUpType.ExpandPaddle;
        

        public override void Activate()
        {
            base.Activate();

            Level.Current.Paddle.Expand(_multiplyer);
        }

        public override void Deactivate()
        {
            base.Deactivate();

            Level.Current.Paddle.Schrink(_multiplyer);
        }
    }
}