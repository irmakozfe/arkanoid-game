using GArkanoid.Systems;
using Godot;

namespace Garkanoid.State
{
    // partial class -> nodedan inherit olmus classlar icin (Godot) o yuzden burda kullanmadik
    public class InGameState : GameStateBase
    {
        public override GameStateType StateType => GameStateType.InGame;

        public override string ScenePath => "res://Scene/Levels/Level.tscn";

        // public InGameState(): base() {} -> base is here optional, godot makes it under the hood 
        public InGameState(): base()
        {
            AddTargetState(GameStateType.Pause);
            AddTargetState(GameStateType.Win);
            AddTargetState(GameStateType.GameOver);
            AddTargetState(GameStateType.InGame); // for level completion, forceLoad = true

        }


    }

}