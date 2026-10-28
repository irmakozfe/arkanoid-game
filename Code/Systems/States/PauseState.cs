using Garkanoid.State;
using GArkanoid.Systems;
using Godot;

namespace Garkanoid.Pause
{

    public class PauseState : GameStateBase
    {
        public override GameStateType StateType => GameStateType.Pause;

        public override string ScenePath => "res://Scene/Menus/Settings.tscn";

        public override bool IsAdditive => true;

        public PauseState()
        {
            AddTargetState(GameStateType.InGame);
            AddTargetState(GameStateType.MainMenu);
        }

        public override void OnEnter(bool forceLoad = false)
        {
            base.OnEnter(forceLoad);
            // Pause game when entering this state.

            GameManager.Instance.SceneTree.Paused = true;
    
        }

        public override void OnExit(bool keepLoaded)
        {
            base.OnExit(keepLoaded);

            // resume the game when the state is deactivated
            GameManager.Instance.SceneTree.Paused = false;
        }
    }
}