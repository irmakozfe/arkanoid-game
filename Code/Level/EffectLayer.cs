using Godot;

namespace Garkanoid.Effects
{
    // used for spawning the correct effect
    public enum EffectType
    {
        None =0,
        Bounce, // collision with a wall or the paddle
        Hit, // collision with a block 
        Death // destruction of the ball
        
    }
    public partial class EffectLayer: Node2D
    {
        
    }    
}