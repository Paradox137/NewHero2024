using System.Collections.Generic;
using System.Threading.Tasks;
using HeroScripts.Enemy;
using HeroScripts.Infrastructure.AssetManagement;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using HeroScripts.Infrastructure.States;
using HeroScripts.Logic;
using HeroScripts.StaticData;
using HeroScripts.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.AI;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace HeroScripts.Infrastructure.Factory
{
	public class GameFactory : IGameFactory
	{
		private GameObject HeroGameObject { get; set; }
		
		private readonly IAssetsProvider _assetProvider;
		private readonly IStaticDataService _staticData;
		private readonly IRandomService _randomService;
		private readonly IPersistentProgressService _persistentProgressService;
		private readonly IGameStateMachine _gameStateMachine;

		public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
		public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
		
		public GameFactory(IAssetsProvider assetProvider, IStaticDataService staticData, IRandomService randomService, 
			IPersistentProgressService persistentProgressService, IGameStateMachine gameStateMachine)
		{
			_assetProvider = assetProvider;
			_staticData = staticData;
			_randomService = randomService;
			_persistentProgressService = persistentProgressService;
			_gameStateMachine = gameStateMachine;
		}
		public GameObject CreateHero(Vector3 at)
		{
			GameObject hero =InstantiateRegistered(AssetsPath.HeroPath, at);
			HeroGameObject = hero;
			return hero;
		}
		
		public GameObject CreateHud()
		{
			GameObject hud = InstantiateRegistered(AssetsPath.HudPath);
			
			hud.GetComponentInChildren<LootCounter>().Construct(_persistentProgressService.Progress.WorldData);

			return hud;
		}
		public async Task<GameObject> CreateEnemy(EnemyTypeID enemyTypeID, Transform parent)
		{
			MonsterStaticData enemyData = _staticData.ForEnemy(enemyTypeID);

			AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(enemyData.PrefabReference);
			
			GameObject enemyPrefab = await handle.Task;
			
			Addressables.Release(handle);
			
			GameObject enemy = Object.Instantiate(enemyPrefab, parent.position, Quaternion.identity, parent);

			IHealth health = enemy.GetComponent<IHealth>();
			health.Current = enemyData.HP;
			health.Max = enemyData.HP;
			
			enemy.GetComponent<ActorUI>().Construct(health);
			enemy.GetComponent<NavMeshAgent>().speed = enemyData.MoveSpeed;


			LootSpawner lootSpawner = enemy.GetComponentInChildren<LootSpawner>();
			lootSpawner.SetLoot(enemyData.MinLoot, enemyData.MaxLoot);
			lootSpawner.Construct(this, _randomService);
			
			Attack attack = enemy.GetComponent<Attack>();
			attack.Construct(HeroGameObject.transform);
			attack.Damage = enemyData.Damage;
			attack.EffectiveDistance = enemyData.EffectiveDistance;
			attack.RadiusAttackHit = enemyData.RadiusAttackHit;

			enemy.GetComponent<AgentMoveToHero>()?.Construct(HeroGameObject.transform);
			enemy.GetComponent<RotateToHero>()?.Construct(HeroGameObject.transform);
			
			return enemy;
		}
		public LootEntity CreateLoot()
		{
			var lootEntity = InstantiateRegistered(AssetsPath.Loot).GetComponent<LootEntity>();
			lootEntity.Construct(_persistentProgressService.Progress.WorldData);
			
			return lootEntity;
		}
		public void CreateSpawner(Vector3 at, string spawnerID, EnemyTypeID EnemyTypeID)
		{
			var spawner = InstantiateRegistered(AssetsPath.Spawner, at).GetComponent<SpawnPoint>();

			spawner.Construct(this);
			spawner.ID = spawnerID;
			spawner.EnemyTypeID = EnemyTypeID;
		}
		public void CreateLevelTransfer(Vector3 at)
		{
			GameObject prefab = InstantiateRegistered(AssetsPath.LevelTransferTrigger, at);
			LevelTransferTrigger levelTransfer = prefab.GetComponent<LevelTransferTrigger>();
      
			levelTransfer.Construct(_gameStateMachine);
		}
		public void CleanUp()
		{
			ProgressReaders.Clear();
			ProgressWriters.Clear();
		}
		public void Register(ISavedProgressReader progressReader)
		{
			if (progressReader is ISavedProgress progressWriter)
				ProgressWriters.Add(progressWriter);
			
			ProgressReaders.Add(progressReader);
		}

		private GameObject InstantiateRegistered(string prefabPath, Vector3 position)
		{
			GameObject gameObject = _assetProvider.Instantiate(prefabPath, position);

			foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
				Register(progressReader);
			
			return gameObject;
		}
		private GameObject InstantiateRegistered(string prefabPath)
		{
			GameObject gameObject = _assetProvider.Instantiate(prefabPath);

			foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
				Register(progressReader);
			
			return gameObject;
		}
	}
}
