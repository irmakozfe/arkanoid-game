using GArkanoid.Systems;

namespace Garkanoid.State
{
    public class GameOverState : GameStateBase
    {
        public override GameStateType StateType => GameStateType.GameOver;

        public override string ScenePath => "res://Scene/Menus/GameOver.tscn";

        public GameOverState()
        {
            AddTargetState(GameStateType.MainMenu);
            AddTargetState(GameStateType.InGame);
        }


    }
}