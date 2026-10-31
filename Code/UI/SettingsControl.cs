using Garkanoid.State;
using GArkanoid.Systems;
using Godot;

namespace Garkanoid.UI
{
	public partial class SettingsControl : Control
	{
		// TODO: implement the save state by yourself!
		[Export] private Button _cancelButton = null;
		[Export] private Button _mainMenuButton = null;

		#region Audio

		[Export] private AudioControl _masterAudioControl = null;
		[Export] private AudioControl _musicAudioControl = null;
		[Export] private AudioControl _sfxAudioControl = null;

		[Export] private string _masterBusName = null;
		[Export] private string _musicBusName = null;
		[Export] private string _sfxBusName = null;
		[Export] private string _masterDisplayName = null;
		[Export] private string _musicDisplayName = null;
		[Export] private string _sfxDisplayName = null;

		#endregion Audio

		#region Resolution

		[Export] private ResolutionControl _resolutionControl = null;
		[Export] private string _resolutionDisplayName = "Resolution";
		private static readonly Vector2I DesignResolution = new(640, 360);

		#endregion Resolution

		public override void _EnterTree()
		{
			if (_cancelButton != null)
			{
				_cancelButton.Pressed += OnClosePressed;
			}

			if (_mainMenuButton != null)
			{
				_mainMenuButton.Pressed += OnMainMenuPressed;
			}

			if (_masterAudioControl != null)
			{
				_masterAudioControl.VolumeChanged += OnVolumeChanged;
			}

			if (_musicAudioControl != null)
			{
				_musicAudioControl.VolumeChanged += OnVolumeChanged;
			}

			if (_sfxAudioControl != null)
			{
				_sfxAudioControl.VolumeChanged += OnVolumeChanged;
			}

			if (_resolutionControl != null)
			{
				_resolutionControl.ResolutionChanged += OnResolutionChanged;
				_resolutionControl.FullscreenToggled += OnFullscreenToggled;
			}
		}

		public override void _ExitTree()
		{
			if (_cancelButton != null)
			{
				_cancelButton.Pressed -= OnClosePressed;
			}

			if (_mainMenuButton != null)
			{
				_mainMenuButton.Pressed -= OnMainMenuPressed;
			}

			if (_masterAudioControl != null)
			{
				_masterAudioControl.VolumeChanged -= OnVolumeChanged;
			}

			if (_musicAudioControl != null)
			{
				_musicAudioControl.VolumeChanged -= OnVolumeChanged;
			}

			if (_sfxAudioControl != null)
			{
				_sfxAudioControl.VolumeChanged -= OnVolumeChanged;
			}

			if (_resolutionControl != null)
			{
				_resolutionControl.ResolutionChanged -= OnResolutionChanged;
				_resolutionControl.FullscreenToggled -= OnFullscreenToggled;
			}
		}

		public override void _Ready()
		{
			// TODO: when settings data is implemented, read this from the data.
			if (_masterAudioControl != null)
			{
				SetupAudioControl(_masterAudioControl, _masterBusName, _masterDisplayName);
			}

			if (_musicAudioControl != null)
			{
				SetupAudioControl(_musicAudioControl, _musicBusName, _musicDisplayName);
			}

			if (_sfxAudioControl != null)
			{
				SetupAudioControl(_sfxAudioControl, _sfxBusName, _sfxDisplayName);
			}

			if (_resolutionControl != null)
			{
				Window window = GetWindow();
				bool isFullscreen = window.Mode == Window.ModeEnum.Fullscreen;

				_resolutionControl.Setup(_resolutionDisplayName, DesignResolution, window.Size, isFullscreen);
			}
		}

		private void SetupAudioControl(AudioControl audioControl, string busName, string displayName)
		{
			int audioBusIndex = AudioServer.GetBusIndex(busName);

			if (audioBusIndex >= 0)
			{
				float volume = AudioServer.GetBusVolumeDb(audioBusIndex);
				audioControl.Setup(busName, displayName, volume);
			}
		}

		private void OnClosePressed()
		{
			if (!GameManager.Instance.ActivatePreviousState())
			{
				GD.PushError("Game can not be continued");
			}
		}

		private void OnMainMenuPressed()
		{
			GetTree().Paused = false;

			if (!GameManager.Instance.ChangeState(GameStateType.MainMenu))
			{
				GD.PushError("Could not return to Main Menu.");
			}
		}

		private void OnVolumeChanged(string audioBus, float decibel)
		{
			int audioBusIndex = AudioServer.GetBusIndex(audioBus);

			if (audioBusIndex < 0)
			{
				return;
			}

			AudioServer.SetBusVolumeDb(audioBusIndex, decibel);
		}

		private void OnResolutionChanged(Vector2I resolution)
		{
			DisplayServer.WindowSetSize(resolution);

			int screen = DisplayServer.WindowGetCurrentScreen();
			Vector2I screenPosition = DisplayServer.ScreenGetPosition(screen);
			Vector2I screenSize = DisplayServer.ScreenGetSize(screen);
			DisplayServer.WindowSetPosition(screenPosition + (screenSize - resolution) / 2);
		}

		private void OnFullscreenToggled(bool fullscreen)
		{
			if (fullscreen)
			{
				GetWindow().Mode = Window.ModeEnum.Fullscreen;
			}
			else
			{
				GetWindow().Mode = Window.ModeEnum.Windowed;
				OnResolutionChanged(_resolutionControl.SelectedResolution);
			}
		}
	}
}
