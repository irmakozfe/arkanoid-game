using GArkanoid.Systems;

namespace Garkanoid.State
{
    // Pause & Settings State
    public class SettingsState : GameStateBase
    {
        public override GameStateType StateType => GameStateType.Settings;

        public override string ScenePath => "res://Scene/Menus/Settings.tscn";

        public SettingsState()
        {
            AddTargetState(GameStateType.MainMenu);
        }

        public override bool IsAdditive => true;


    }
}