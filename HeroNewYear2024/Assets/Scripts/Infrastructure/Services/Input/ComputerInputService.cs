using UnityEngine;

namespace HeroScripts.Services.Input
{
	public class ComputerInputService : InputService
	{
		public override Vector2 Axis => GetInputAxis();
		
		protected override Vector2 GetInputAxis() => 
			new Vector2(UnityEngine.Input.GetAxis(Horizontal), UnityEngine.Input.GetAxis(Vertical));
	}
}
