using Godot;

namespace Garkanoid.PowerUps
{
    [GlobalClass]
    public partial class PowerUpSceneMapping : Resource
    {
        [Export] public PowerUpType PowerUpType {get; set;} = PowerUpType.None;
        [Export] public PackedScene PowerUpScene {get;set;} = null;
    }
}