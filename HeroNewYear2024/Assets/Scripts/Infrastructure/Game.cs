using HeroScripts.Logic;
using HeroScripts.Services.Input;
using UnityEngine;

namespace HeroScripts.Infrastructure
{
	public class Game
	{
		public static IInputService InputService;
		public GameStateMachine StateMachine;

		public Game(ICoroutineRunner __coroutineRunner, LoadingCurtain __loadingCurtain)
		{
			StateMachine = new GameStateMachine(new SceneLoader(__coroutineRunner),
				__loadingCurtain);
		}
	}
}
