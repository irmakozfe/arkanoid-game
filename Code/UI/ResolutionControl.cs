using System;
using System.Collections.Generic;
using Godot;

namespace Garkanoid.UI
{
	public partial class ResolutionControl: VBoxContainer
	{
		[Signal]
		public delegate void ResolutionChangedEventHandler(Vector2I resolution);

		[Export] private Label _nameLabel = null;
		[Export] private OptionButton _resolutionOption = null;

		private readonly List<Vector2I> _resolutions = new();

		public override void _EnterTree()
		{
			if (_resolutionOption != null)
			{
				_resolutionOption.ItemSelected += OnItemSelected;
			}
		}

		public override void _ExitTree()
		{
			if (_resolutionOption != null)
			{
				_resolutionOption.ItemSelected -= OnItemSelected;
			}
		}

		public void Setup(string displayName, Vector2I designSize, Vector2I currentSize)
		{
			if (_nameLabel != null)
			{
				_nameLabel.Text = displayName;
			}

			_resolutions.Clear();
			_resolutionOption.Clear();

			Vector2I screenSize = DisplayServer.ScreenGetSize();
			int max = Mathf.Min(screenSize.X / designSize.X, screenSize.Y / designSize.Y);
			max = Mathf.Max(1, max);

			for (int scale =1 ; scale <= max; scale++)
			{
				Vector2I resolution = designSize * scale;
				_resolutions.Add(resolution);
				_resolutionOption.AddItem($"{resolution.X}x{resolution.Y}");
			}

			int currentIndex = _resolutions.IndexOf(currentSize);
			_resolutionOption.Select(
				currentIndex >= 0 
				? currentIndex
				: 0
			);
			
		}

		private void OnItemSelected(long index)
		{
			EmitSignal(SignalName.ResolutionChanged, _resolutions[(int) index]);
		}
	
	}
}
