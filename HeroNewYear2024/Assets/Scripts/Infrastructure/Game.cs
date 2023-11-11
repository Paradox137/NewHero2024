using HeroScripts.Services.Input;
using UnityEngine;

namespace HeroScripts.Infrastructure
{
	public class Game
	{
		public static IInputService InputService;

		public Game()
		{
			RegisterInput();
		}
		private static void RegisterInput()
		{
			/*if (Application.isMobilePlatform)
				InputService = new MobileInputService();
			else*/
			InputService = new ComputerInputService();
		}
	}
}
