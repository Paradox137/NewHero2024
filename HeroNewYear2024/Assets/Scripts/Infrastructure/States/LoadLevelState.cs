using System.Collections.Generic;
using System.Xml;
using HeroScripts.CameraLogic;
using HeroScripts.Data;
using HeroScripts.Enemy;
using HeroScripts.Hero;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using HeroScripts.Logic;
using HeroScripts.StaticData;
using HeroScripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

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
		private readonly IStaticDataService _staticDataService;

		public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader, LoadingCurtain loadingCurtain, IGameFactory gameFactory, 
			IPersistentProgressService progressService, IStaticDataService staticDataService)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;
			_loadingCurtain = loadingCurtain;
			_gameFactory = gameFactory;
			_progressService = progressService;
			_staticDataService = staticDataService;
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
			InitLootEntities();
			
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
			string sceneKey = SceneManager.GetActiveScene().name;
			LevelStaticData levelData = _staticDataService.ForLevel(sceneKey);

			foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
			{
				_gameFactory.CreateSpawner(spawnerData.Position, spawnerData.ID, spawnerData.EnemyTypeID);
			}
		}
		
		private void InitLootEntities()
		{
			foreach (KeyValuePair<string, LootEntityData> item in _progressService.Progress.WorldData.LootData.LootEntitiesOnScene.Dictionary)
			{
				LootEntity lootPiece = _gameFactory.CreateLoot();
				lootPiece.GetComponent<UniqueID>().ID = item.Key;
				lootPiece.Initialize(item.Value.Loot);
				lootPiece.transform.position = item.Value.Position.AsUnityVector3();
			}
		}
	}
}
