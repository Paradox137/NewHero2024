using HeroScripts.CameraLogic;
using HeroScripts.Hero;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using HeroScripts.Logic;
using HeroScripts.UI;
using UnityEngine;

namespace HeroScripts.Infrastructure
{
	public class LoadLevelState : IPayloadedState<string>
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly LoadingCurtain _loadingCurtain;
		
		private const string InitialPointTag = "InitialPoint";
		private const string SpawnerTag = "EnemySpawner";
		private readonly IGameFactory _gameFactory;
		private readonly IPersistentProgressService _progressService;

		public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory, IPersistentProgressService progressService)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
			_gameFactory = gameFactory;
			_progressService = progressService;
		}

		public void Enter(string sceneName)
		{
			_loadingCurtain.Show();
			_gameFactory.CleanUp();
			_sceneLoader.Load(sceneName, onLoaded);
		}
		public void Exit()
		{
			_loadingCurtain.Hide();
		}
		private void onLoaded()
		{
			InitGameWorld();
			InformProgressReaders();
			
			_gameStateMachine.Enter<GameLoopState>();
		}
		private void InformProgressReaders()
		{
			foreach (ISavedProgressReader progressReader in _gameFactory.ProgressReaders)
			{
				progressReader.LoadProgress(_progressService.Progress);
			}
		}
		private void InitGameWorld()
		{
			InitSpawners();
			
			GameObject hero = _gameFactory.CreateHero(GameObject.FindWithTag(InitialPointTag));

			var hud = _gameFactory.CreateHud();
			
			hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());

			CameraFollow(hero);
		}

		private void CameraFollow(GameObject hero)
		{
			if (Camera.main != null)
				Camera.main.GetComponentInParent<CameraFollow>().Follow(hero);
		}

		private void InitSpawners()
		{
			foreach (GameObject spawnerGO in GameObject.FindGameObjectsWithTag(SpawnerTag))
			{
				EnemySpawner spawner = spawnerGO.GetComponent<EnemySpawner>();
				
				_gameFactory.Register(spawner);
			}
		}
	}
}
