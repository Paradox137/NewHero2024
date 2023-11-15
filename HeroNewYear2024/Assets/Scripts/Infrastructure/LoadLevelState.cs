using HeroScripts.CameraLogic;
using UnityEngine;

namespace HeroScripts.Infrastructure
{
	public class LoadLevelState : IPayloadedState<string>
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly SceneLoader _sceneLoader;
		private const string InitialPointTag = "InitialPoint";

		public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;

		}
			
		public void Enter(string sceneName)
		{
			_sceneLoader.Load(sceneName, onLoaded);
		}
		public void Exit()
		{
		}
		private void onLoaded()
		{
			GameObject initialPoint = GameObject.FindWithTag(InitialPointTag);
			GameObject hero = Instantiate("Hero/hero", initialPoint.transform.position);
			
			Instantiate("Hud/hud");

			CameraFollow(hero);
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
