using HeroScripts.Logic;
using UnityEngine;

namespace HeroScripts.Infrastructure
{
	public class GameBootstrapper : MonoBehaviour, ICoroutineRunner
	{
		private Game _game;
		public LoadingCurtain CurtainPrefab;
		private void Awake()
		{
			_game = new Game(this, Instantiate(CurtainPrefab));
			
			_game.StateMachine.Enter<BootstrapState>();
			
			DontDestroyOnLoad(this);
		}
	}
}
