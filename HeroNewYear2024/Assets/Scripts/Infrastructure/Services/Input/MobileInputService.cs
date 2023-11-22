using UnityEngine;

namespace HeroScripts.Services.Input
{
	public class MobileInputService : InputService
	{
		private readonly FloatingJoystick _joystick;

		public MobileInputService(FloatingJoystick __joystick)
		{
			_joystick = __joystick;
		}
		
		public override Vector2 Axis => GetInputAxis();
		protected override Vector2 GetInputAxis() => new Vector2(_joystick.Horizontal, _joystick.Vertical);
	}
}
