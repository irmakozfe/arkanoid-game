using GArkanoid.Systems;
using Godot;

namespace Garkanoid.State
{
    public class MainMenuState : GameStateBase
    {
        public override GameStateType StateType => GameStateType.MainMenu;

        public override string ScenePath => "res://Scene/Menus/MainMenu.tscn";

        public MainMenuState()
        {
            AddTargetState(GameStateType.InGame);
            AddTargetState(GameStateType.Settings);
        }


    }

}