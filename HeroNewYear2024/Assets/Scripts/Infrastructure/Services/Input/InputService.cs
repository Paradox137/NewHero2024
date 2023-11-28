using UnityEngine;

namespace HeroScripts.Services.Input
{
	public abstract class InputService : IInputService
	{
		protected const string Horizontal = "Horizontal";
		protected const string Vertical = "Vertical";
		
		public abstract Vector2 Axis { get; }
		public bool IsAttackButtonUp()
		{
			return UnityEngine.Input.GetKeyDown(KeyCode.F);
		}

		protected abstract  Vector2 GetInputAxis();

	}
}
