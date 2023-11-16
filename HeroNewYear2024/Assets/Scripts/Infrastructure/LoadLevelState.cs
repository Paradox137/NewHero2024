using HeroScripts.CameraLogic;
using HeroScripts.Logic;
using UnityEngine;

namespace HeroScripts.Infrastructure
{
	public class LoadLevelState : IPayloadedState<string>
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		
		private const string InitialPointTag = "InitialPoint";
		private readonly IGameFactory _gameFactory;

		public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
		}

		public void Enter(string sceneName)
		{
			_loadingCurtain.Show();
			_sceneLoader.Load(sceneName, onLoaded);
		}
		public void Exit()
		{
			_loadingCurtain.Hide();
		}
		private void onLoaded()
		{
			GameObject hero = _gameFactory.CreateHero(GameObject.FindWithTag(InitialPointTag));

			_gameFactory.CreateHud();

			CameraFollow(hero);
			
			_gameStateMachine.Enter<GameLoopState>();
		}

		private void CameraFollow(GameObject hero)
		{
			if (Camera.main != null)
				Camera.main.GetComponentInParent<CameraFollow>().Follow(hero);
		}
	}
}
