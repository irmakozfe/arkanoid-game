using GArkanoid.Systems;

namespace Garkanoid.State
{
    public class WinState : GameStateBase
    {
        public override GameStateType StateType => GameStateType.Win;

        public override string ScenePath => "res://Scene/Menus/Win.tscn";

        public WinState()
        {
            AddTargetState(GameStateType.MainMenu);
            AddTargetState(GameStateType.InGame);
        }
    }
}