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
		private const string HeroPath = "Hero/hero";
		private const string HudPath = "Hud/hud";

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
			GameObject initialPoint = GameObject.FindWithTag(InitialPointTag);
			GameObject hero = Instantiate(HeroPath, initialPoint.transform.position);

			Instantiate(HudPath);

			CameraFollow(hero);
			
			_gameStateMachine.Enter<GameLoopState>();
		}

		private void CameraFollow(GameObject hero)
		{
			if (Camera.main != null)
				Camera.main.GetComponentInParent<CameraFollow>().Follow(hero);
		}

		private static GameObject Instantiate(string path)
		{
			var prefab = Resources.Load<GameObject>(path);

			return Object.Instantiate(prefab);
		}

		private static GameObject Instantiate(string path, Vector3 at)
		{
			var prefab = Resources.Load<GameObject>(path);

			return Object.Instantiate(prefab, at, Quaternion.identity);
		}
	}
}
