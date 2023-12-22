using System.Collections.Generic;
using System.Threading.Tasks;
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
			_gameFactory.WarmUp();
			_sceneLoader.Load(sceneName, onLoaded);
		}
		public void Exit()
		{
			_loadingCurtain.Hide();
		}
		private async void onLoaded()
		{
			await InitGameWorld();
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
		private async Task InitGameWorld()
		{
			LevelStaticData levelData = _staticDataService.ForLevel(SceneManager.GetActiveScene().name);
			await InitSpawners(levelData);
			await InitLootEntities();
			GameObject hero = await InitHero(levelData);
			await InitLevelTransfer(levelData);
			await InitHud(hero);
			CameraFollow(hero);
		}
		private async Task InitHud(GameObject hero)
		{
			var hud = await _gameFactory.CreateHud();

			hud.GetComponentInChildren<ActorUI>().Construct(hero.GetComponent<HeroHealth>());
		}
		private async Task<GameObject> InitHero(LevelStaticData levelData) 
		{
			return  await _gameFactory.CreateHero(levelData.InitialHeroPosition);
		}
		private Task InitLevelTransfer(LevelStaticData levelData) 
		{
			return _gameFactory.CreateLevelTransfer(levelData.LevelTransfer.Position);
		}

		private void CameraFollow(GameObject hero)
		{
			if (Camera.main != null)
				Camera.main.GetComponentInParent<CameraFollow>().Follow(hero);
		}

		private async Task InitSpawners(LevelStaticData levelData)
		{
			foreach (EnemySpawnerData spawnerData in levelData.EnemySpawners)
			{
				await _gameFactory.CreateSpawner(spawnerData.Position, spawnerData.ID, spawnerData.EnemyTypeID);
			}
		}
		
		private async Task InitLootEntities()
		{
			foreach (KeyValuePair<string, LootEntityData> item in _progressService.Progress.WorldData.LootData.LootEntitiesOnScene.Dictionary)
			{
				LootEntity lootPiece = await _gameFactory.CreateLoot();
				lootPiece.GetComponent<UniqueID>().ID = item.Key;
				lootPiece.Initialize(item.Value.Loot);
				lootPiece.transform.position = item.Value.Position.AsUnityVector3();
			}
		}
	}
}
