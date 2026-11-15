using Garkanoid.State;
using GArkanoid.Systems;
using Godot;

namespace Garkanoid.UI
{
	public partial class SettingsControl : Control
	{
		[Export] private Button _applyButton = null;          
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
		private SettingsData _working;

		public override void _EnterTree()
		{
			_working = Settings.Current;                        

			if (_applyButton != null)                           
			{
				_applyButton.Pressed += OnApplyPressed;
			}

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
			if (_applyButton != null)                           
			{
				_applyButton.Pressed -= OnApplyPressed;
			}

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
			if (_masterAudioControl != null)
			{
				SetupAudioControl(_masterAudioControl, _masterBusName, _masterDisplayName, _working.MasterVolume);   // ← DEĞİŞTİ
			}

			if (_musicAudioControl != null)
			{
				SetupAudioControl(_musicAudioControl, _musicBusName, _musicDisplayName, _working.MusicVolume);       // ← DEĞİŞTİ
			}

			if (_sfxAudioControl != null)
			{
				SetupAudioControl(_sfxAudioControl, _sfxBusName, _sfxDisplayName, _working.SFXVolume);              // ← DEĞİŞTİ
			}

			if (_resolutionControl != null)
			{
				_resolutionControl.Setup(_resolutionDisplayName, DesignResolution, _working.Resolution, _working.Fullscreen);
			}
		}

		private void SetupAudioControl(AudioControl audioControl, string busName, string displayName, float volume)
		{
			audioControl.Setup(busName, displayName, volume);
		}

		private void OnApplyPressed()
		{
			Settings.Save(_working);

			if (!GameManager.Instance.ActivatePreviousState())
			{
				GD.PushError("Scene can not be continued.");
			}
		}

		private void OnClosePressed()
		{
			RevertUnsavedChanges();                             

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
		private void RevertUnsavedChanges()
		{
			Settings.Apply(Settings.Current);
			_working = Settings.Current;
		}

		private void OnVolumeChanged(string audioBus, float decibel)
		{
			if (audioBus == _masterBusName)
			{
				_working.MasterVolume = decibel;
			}
			else if (audioBus == _musicBusName)
			{
				_working.MusicVolume = decibel;
			}
			else if (audioBus == _sfxBusName)
			{
				_working.SFXVolume = decibel;
			}

			int audioBusIndex = AudioServer.GetBusIndex(audioBus);

			if (audioBusIndex < 0)
			{
				return;
			}

			AudioServer.SetBusVolumeDb(audioBusIndex, decibel);
		}

		private void OnResolutionChanged(Vector2I resolution)
		{
			_working.Resolution = resolution;
			Settings.Apply(_working);   
		}

		private void OnFullscreenToggled(bool fullscreen)
		{
			_working.Fullscreen = fullscreen;

			if (!fullscreen)
			{
				_working.Resolution = _resolutionControl.SelectedResolution;
			}

			Settings.Apply(_working);   
		}
	}
}
