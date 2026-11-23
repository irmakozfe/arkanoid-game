using Garkanoid.Save;
using Garkanoid.State;
using GArkanoid.Entities;
using Godot;
using System;
using System.Collections.Generic;

// responsible for state machines , contains game data

namespace GArkanoid.Systems{

public partial class GameManager : Node
{
	[ExportGroup("Music")]
	[Export] private AudioStream _menuMusic;
	[Export] private AudioStream _gameMusic;

	private AudioStreamPlayer _musicPlayer;

	#region Singleton
		public static GameManager Instance
		{
			get;
			private set;
		}

		public GameManager()
		{
			GD.Print("Initializing Game Manager");
			if (Instance == null)
			{
				Instance = this;
			}
			else if (Instance != this)
			{
				QueueFree();
				return;
			}
		}

		//singleton functionality: 
		public sealed override void _Ready()  // sealed: no further overrides are possible
		{
			SceneTree.CurrentScene.QueueFree();

			GameStateBase mainMenu = GetState(GameStateType.MainMenu);
			mainMenu.OnEnter();
			LoadedStates.Push(mainMenu);

			Settings.Initialize();                                        

			_musicPlayer = new AudioStreamPlayer
			{
				Name = "MusicPlayer",
				Bus = "Music",
				ProcessMode = ProcessModeEnum.Always
			};
			AddChild(_musicPlayer);
			UpdateMusic(GameStateType.MainMenu);

		}

		#endregion

		// naming is important ....EventHandler();
		[Signal]
		public delegate void ScoreChangedEventHandler(int score);
		
		[Signal]
		public delegate void LivesChangedEventHandler(int lives); 
		
		private int _score = Config.InitialScore;
		private int _lives = Config.InitialLives;

		private SceneTree _sceneTree = null;

		// contains all states that are currently loaded.
		private Stack<GameStateBase> _loadedStates = new Stack<GameStateBase>();

		public int Lives
		{
			get{return _lives;}
			private set
			{
				_lives = Mathf.Max(value, 0);
				GD.Print($"Lives: {_lives}");
				EmitSignal(SignalName.LivesChanged, _lives);
			}
		}

		public SceneTree SceneTree
		{
			get
			{
				// Lazy loading : only computed when accessed for the first time 
				if (_sceneTree == null)
				{
					_sceneTree = GetTree();
				}

				return _sceneTree;
			}
		}

		public SaveSystem SaveSystem {get; private set;} = new SaveSystem();

		// all states derives from GameStateBase so we use GameStateBase
		private Dictionary<GameStateType, GameStateBase> _gameStates = new Dictionary<GameStateType, GameStateBase>
		{
			{ GameStateType.MainMenu, new MainMenuState() },
			{ GameStateType.InGame, new InGameState() },
			{ GameStateType.GameOver, new GameOverState() },
			{ GameStateType.Win, new WinState() },
			{ GameStateType.Settings, new SettingsState() },
			{ GameStateType.Pause, new PauseState() }
		}; 

		public Stack<GameStateBase> LoadedStates { get; private set; } = new Stack<GameStateBase>();
		public GameStateBase ActiveState
		{
			get {
				return LoadedStates.Count > 0 
				? LoadedStates.Peek() 
				: null;
			}
		}

		private GameStateBase GetState(GameStateType gameStateType)
		{
			return _gameStates.TryGetValue(gameStateType, out GameStateBase state) ? state : null;
		} 

		// to make the transitions -> make the method async  (coroutine)
		public bool ChangeState(GameStateType targetStateType, bool force = false)
		{
			// check transition to the next state is possible (are there any errors, is it valid?)
			if(ActiveState != null && !ActiveState.CanTransitionTo(targetStateType))
			{
				GD.PushError($"Transision {ActiveState.StateType} was not legal");
				return false;
			}

			GameStateBase nextState = GetState(targetStateType);
			if(nextState == null)
			{
				GD.PushError($"Next state {targetStateType} is not found!");
				return false;
			}

			if (nextState.IsAdditive)
			{
				// ? -> execute the method only when the active state is not null 
				ActiveState?.OnExit(keepLoaded: true);
			} else
			{
				while(LoadedStates.Count > 0)
				{
					GameStateBase poppedState = LoadedStates.Pop();
					poppedState.OnExit(keepLoaded: false);
				}
			}

			LoadedStates.Push(nextState);
			ActiveState.OnEnter(force);

			UpdateMusic(targetStateType);

			return true;
		}	

		// exit from pause or settings menu to the prevous menu
		// TODO: refactor changestate method to support this corner case and get rid of this method
		public bool ActivatePreviousState()
		{
		if (LoadedStates.Count < 2)
			{
				GD.PushError("No previous state to activate");
				return false;
			}	

			// Pop the current state from the stack
			GameStateBase previousState = LoadedStates.Pop();
			previousState.OnExit(keepLoaded:false);
			
			// activate the already loaded, new active state
			ActiveState.OnEnter();

			return true;
		}

		public override void _Input(InputEvent @event)
		{
			if (@event.IsActionPressed(Config.QuickSaveAction))
			{
				QuickSave();
			}else if (@event.IsActionPressed(Config.QuickLoadAction))
			{
				QuickLoad();
			}
		}

		private void QuickLoad()
		{
			var loadData = SaveSystem.QuickLoad();

			if(loadData == null)
			{
				GD.PushError("Load data is empty");
				return;
			}

			Score = (int) loadData[Config.ScoreKey];
			Lives = (int) loadData[Config.LivesKey];

			// force reload 
			ChangeState(GameStateType.InGame, force: true);

		}
		private void QuickSave()
		{
			if (ActiveState?.StateType != GameStateType.InGame || Level.Current == null)
			{
				GD.PushWarning("Quick save is only possible in game.");
				return;
			}
			var data = new Godot.Collections.Dictionary<string, Variant>
			{
				{Config.ScoreKey, Score},
				{Config.LivesKey, Lives}
			};

			Level.Current.Save(ref data);

			SaveSystem.QuickSave(data);
			
		}

		public int Score
		{
			get{return _score;}
			private set
			{
				// TODO validate the score value 
				_score = value;
				EmitSignal(SignalName.ScoreChanged, _score);
				// TODO notify about updating the score!
			}
		}

		protected virtual void Initialize()
		{
			GD.Print("GameManager is initialized.");
		}

		public void AddScore(int scoreToAdd)
		{
			if (scoreToAdd <= 0)
			{
				return;
			}
			
			Score += scoreToAdd;

			
		}

		public bool RemoveLife()
		{

			if (Lives <= 0) return false;

			Lives--;

			if(Lives == 0)
			{
				GD.Print("Game Over!");
				ChangeState(GameStateType.GameOver);

				return false;
			}

			return true;
		}

		public void ResetGame()
		{
			Score =0;
			Lives = 3; 
		}

		private void UpdateMusic(GameStateType stateType)
		{
			switch (stateType)
			{
				case GameStateType.MainMenu:
				PlayMusic(_menuMusic);
				break;

				case GameStateType.InGame:
				PlayMusic(_gameMusic);
				break;

				case GameStateType.Pause:
				break;

				case GameStateType.GameOver:
				_musicPlayer.Stop();
				break;
			}
		}

		private void PlayMusic(AudioStream music)
		{
			if(_musicPlayer.Stream == music && _musicPlayer.Playing)
			{
				return;
			}

			_musicPlayer.Stream = music;
			_musicPlayer.Play();
		}

}
}
