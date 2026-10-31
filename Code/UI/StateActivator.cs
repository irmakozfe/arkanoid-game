using System;
using Garkanoid.State;
using GArkanoid.Systems;
using Godot;

namespace Garkanoid.UI
{
	public partial class StateActivator: Node
	{

		[Export] private Godot.Collections.Array<GameStateType> _activeStates = new();
		private Node _target;

		public override void _Ready()
		{
			_target = GetParent();
			Refresh();
		}

		public void Refresh()
		{
			GameStateBase activeState = GameManager.Instance?.ActiveState;

			bool isActive = activeState != null && _activeStates.Contains(activeState.StateType);
			SetActive(isActive);
		}

		private void SetActive(bool active)
		{
			if(_target is CanvasItem canvasItem)
			{
				canvasItem.Visible = active;
			}

			if(_target is BaseButton btn)
			{
				btn.Disabled = !active;
			}

			_target.ProcessMode = active ? ProcessModeEnum.Inherit : ProcessModeEnum.Disabled;
		}
	}
}
