// responsible for state machines , contains game data

namespace GArkanoid.Systems{
    public enum GameStateType
		{
			None=0,
			MainMenu,
			InGame,
			GameOver,
			Win,
			Settings,
			Pause
		}
}
