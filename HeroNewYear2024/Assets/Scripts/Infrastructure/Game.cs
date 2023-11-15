using HeroScripts.Services.Input;
using UnityEngine;

namespace HeroScripts.Infrastructure
{
	public class Game
	{
		public static IInputService InputService;
		public GameStateMachine StateMachine;

		public Game()
		{
			StateMachine = new GameStateMachine();
		}
	}
}
