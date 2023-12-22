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

		public async Task WarmUp()
		{
			await _assetProvider.Load<GameObject>(AssetAddress.Loot);
			await _assetProvider.Load<GameObject>(AssetAddress.Spawner);
		}
		public async Task<GameObject> CreateHero(Vector3 at)
		{
			GameObject hero = await InstantiateRegisteredAsync(AssetAddress.HeroPath, at);
			HeroGameObject = hero;
			return hero;
		}
		
		public async Task<GameObject> CreateHud()
		{
			GameObject hud = await InstantiateRegisteredAsync(AssetAddress.HudPath);
			
			hud.GetComponentInChildren<LootCounter>().Construct(_persistentProgressService.Progress.WorldData);

			return hud;
		}
		public async Task<GameObject> CreateEnemy(EnemyTypeID enemyTypeID, Transform parent)
		{
			MonsterStaticData enemyData = _staticData.ForEnemy(enemyTypeID);

			GameObject enemyPrefab = await _assetProvider.Load<GameObject>(enemyData.PrefabReference);

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
		public async Task<LootEntity> CreateLoot()
		{
			GameObject prefab = await _assetProvider.Load<GameObject>(AssetAddress.Loot);
			
			var lootEntity = InstantiateRegistered(prefab).GetComponent<LootEntity>();
			lootEntity.Construct(_persistentProgressService.Progress.WorldData);
			
			return lootEntity;
		}
		public async Task CreateSpawner(Vector3 at, string spawnerID, EnemyTypeID EnemyTypeID)
		{
			GameObject prefab = await _assetProvider.Load<GameObject>(AssetAddress.Spawner);
			
			SpawnPoint spawner = InstantiateRegistered(prefab, at).GetComponent<SpawnPoint>();

			spawner.Construct(this);
			spawner.ID = spawnerID;
			spawner.EnemyTypeID = EnemyTypeID;
		}
		public async Task CreateLevelTransfer(Vector3 at)
		{
			GameObject prefab = await InstantiateRegisteredAsync(AssetAddress.LevelTransferTrigger, at);
			
			LevelTransferTrigger levelTransfer = prefab.GetComponent<LevelTransferTrigger>();

			levelTransfer.Construct(_gameStateMachine);
		}
		public void CleanUp()
		{
			ProgressReaders.Clear();
			ProgressWriters.Clear();
			
			_assetProvider.Cleanup();
		}
		public void Register(ISavedProgressReader progressReader)
		{
			if (progressReader is ISavedProgress progressWriter)
				ProgressWriters.Add(progressWriter);
			
			ProgressReaders.Add(progressReader);
		}
		private GameObject InstantiateRegistered(GameObject prefab)
		{
			GameObject gameObject = Object.Instantiate(prefab);

			foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
				Register(progressReader);
			
			return gameObject;
		}
		private GameObject InstantiateRegistered(GameObject prefab, Vector3 position)
		{
			GameObject gameObject = Object.Instantiate(prefab, position, Quaternion.identity);

			foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
				Register(progressReader);
			
			return gameObject;
		}
		private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath, Vector3 position)
		{
			GameObject gameObject = await _assetProvider.Instantiate(prefabPath, position);

			foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
				Register(progressReader);
			
			return gameObject;
		}
		private async Task<GameObject> InstantiateRegisteredAsync(string prefabPath)
		{
			GameObject gameObject = await _assetProvider.Instantiate(prefabPath);

			foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
				Register(progressReader);
			
			return gameObject;
		}
	}
}
