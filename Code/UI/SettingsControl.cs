using GArkanoid.Systems;
using Godot;
using System;
using System.Reflection.PortableExecutable;

namespace Garkanoid.UI{
public partial class SettingsControl : Control
	{
		//TODO: implement the save state by yourself!
		[Export] private Button _closeButton = null;

		#region Audio
		
		[Export] private AudioControl _masterAudioControl=null; 
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
			_closeButton.Pressed += OnClosePressed;


			if(_masterAudioControl != null)
			{
				_masterAudioControl.VolumeChanged += OnVolumeChanged;
			}		

			if(_musicAudioControl != null)
			{
				_musicAudioControl.VolumeChanged += OnVolumeChanged;
			}

			if(_sfxAudioControl != null)
			{
				_sfxAudioControl.VolumeChanged += OnVolumeChanged;
			}

			if(_resolutionControl != null)
			{
				_resolutionControl.ResolutionChanged += OnResolutionChanged;
				_resolutionControl.FullscreenToggled += OnFullscreenToggled;
			}
		}

		public override void _ExitTree()
		{
			_closeButton.Pressed -= OnClosePressed;



			if(_masterAudioControl != null)
			{
				_masterAudioControl.VolumeChanged -= OnVolumeChanged;
			}		

			if(_musicAudioControl != null)
			{
				_musicAudioControl.VolumeChanged -= OnVolumeChanged;
			}

			if(_sfxAudioControl != null)
			{
				_sfxAudioControl.VolumeChanged -= OnVolumeChanged;
			}

			if(_resolutionControl != null)
			{
				_resolutionControl.ResolutionChanged -= OnResolutionChanged;
				_resolutionControl.FullscreenToggled -= OnFullscreenToggled;
			}
		}
		


		public override void _Ready()
		{
			if(_masterAudioControl != null)
			{
				// TODO: when settings data is implemented, read this from the data.
				SetupAudioControl(_masterAudioControl, _masterBusName, _masterDisplayName);
			}

			if(_musicAudioControl != null)
			{
				SetupAudioControl(_musicAudioControl, _musicBusName, _musicDisplayName);
			}

			if(_sfxAudioControl != null)
			{
				SetupAudioControl(_sfxAudioControl, _sfxBusName, _sfxDisplayName);
			}

			if(_resolutionControl != null)
			{
				Window window = GetWindow();
				bool isFullscreen = window.Mode == Window.ModeEnum.Fullscreen;

				
				// Vector2I designSize = new(
				// 	(int)ProjectSettings.GetSetting("display/window/size/viewport_width"),
				// 	(int)ProjectSettings.GetSetting("display/window/size/viewport_height")
				// );

				// _resolutionControl.Setup(_resolutionDisplayName, designSize, DisplayServer.WindowGetSize());

				_resolutionControl.Setup(_resolutionDisplayName, DesignResolution, window.Size, isFullscreen);

			}
		}

		private void SetupAudioControl(AudioControl audioControl,  string busName, string displayName)
		{
			int audioBusIndex = AudioServer.GetBusIndex(busName);

			if (audioBusIndex >= 0)
			{
				float masterVolume = AudioServer.GetBusVolumeDb(audioBusIndex);
				audioControl.Setup(busName, displayName, masterVolume);
			}
		}

		private void OnClosePressed()
		{
			if (!GameManager.Instance.ActivatePreviousState())
			{
				GD.PushError("Game can not be continiued");
			}
			
		}

		private void OnVolumeChanged(string audioBus, float decibel)
		{
			// TODO: Add support for previewing audio and then saving or restoring it

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
