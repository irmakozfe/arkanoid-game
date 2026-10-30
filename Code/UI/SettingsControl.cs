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

	}
}
