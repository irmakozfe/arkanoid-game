using Godot;
using System;

namespace Garkanoid.UI{
public partial class AudioControl : HBoxContainer
{
	// audio labels are measured in decibel 
	[Signal]
	 public delegate void VolumeChangedEventHandler(string audioBus, float decibel);

	[Export] private Slider _volumeSlider = null;
	[Export] private Label _nameLabel = null;
	[Export] private Label _volumeLabel = null;

	private string _busName = null;

	public override void _EnterTree()
		{
			if (_volumeSlider != null)
			{
				_volumeSlider.ValueChanged += OnVolumeChanged;
			}
		}

	public override void _ExitTree()
		{
			if (_volumeSlider != null)
			{
				_volumeSlider.ValueChanged -= OnVolumeChanged;
			}
		}

	public void Setup(string busName, string displayName, float decibelVolume)
		{
			//displayName can be translated to another native language thats why they are seperated
			_busName = busName;
			_nameLabel.Text = displayName;
			SetVolume(decibelVolume);

			if (_volumeSlider != null)
			{
				_volumeSlider.ValueChanged += OnVolumeChanged;
			}
		}

	public void SetVolume(float decibelVolume)
		{
			float linearVolume = Mathf.DbToLinear(decibelVolume);

			_volumeSlider.Value = linearVolume;
			_volumeLabel.Text = ((int) (linearVolume * 100)).ToString();
			
		}

	public void UpdateVolume()
		{
			// Volume on a linear scale [0,1], on a decibel [-80, 0]
			float linearVolume = (float) _volumeSlider.Value;

			float decibelVolume=  Mathf.LinearToDb(linearVolume);

			_volumeLabel.Text = ((int) (linearVolume *100) ).ToString();

			EmitSignal(SignalName.VolumeChanged, _busName, decibelVolume);
		}

	private void OnVolumeChanged(double value)
		{
			UpdateVolume();
		}	
}
}
