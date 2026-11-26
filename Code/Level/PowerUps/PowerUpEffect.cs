using Godot;

namespace Garkanoid.PowerUps
{
    public abstract partial class PowerUpEffect : Resource
    {
        [Export] public float Duration {get; protected set;} = 0f;
        public abstract PowerUpType PowerUpType {get;}

        public float TimeRemaining {get; set;} = 0f;
        public bool isActive {get; private set;} = false;

        // ALL WILL BE CALLED FROM THE POWERUPMANAGER
        public virtual void Activate()  
        {
            isActive = true;
            TimeRemaining = Duration;
        }

        public virtual void Deactivate()
        {
            isActive = false;
        }

        public virtual bool Update(float deltaTime)
        {
            // Update applies only to those Powerups that are timed
            if (Duration >= 0)
            {
                TimeRemaining -= deltaTime;
                if(TimeRemaining <= 0)
                {
                    Deactivate();
                }
            }
            return isActive;

        } 

    }
}