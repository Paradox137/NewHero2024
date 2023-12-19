using System.Collections.Generic;
using HeroScripts.Enemy;
using HeroScripts.Infrastructure.AssetManagement;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using HeroScripts.Logic;
using HeroScripts.StaticData;
using HeroScripts.UI;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace HeroScripts.Infrastructure.Factory
{
	public class GameFactory : IGameFactory
	{
		private GameObject HeroGameObject { get; set; }
		
		private readonly IAssetsProvider _assetProvider;
		private readonly IStaticDataService _staticData;

		public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
		public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
		
		public GameFactory(IAssetsProvider assetProvider, IStaticDataService staticData)
		{
			_assetProvider = assetProvider;
			_staticData = staticData;
		}
		public GameObject CreateHero(GameObject at)
		{
			GameObject hero =InstantiateRegistered(AssetsPath.HeroPath, at.transform.position);
			HeroGameObject = hero;
			return hero;
		}
		
		public GameObject CreateHud()
		{
			return InstantiateRegistered(AssetsPath.HudPath);
		}
		public GameObject CreateEnemy(EnemyTypeID enemyTypeID, Transform parent)
		{
			MonsterStaticData enemyData = _staticData.ForEnemy(enemyTypeID);

			GameObject enemy = Object.Instantiate(enemyData.prefab, parent.position, Quaternion.identity, parent);

			var health = enemy.GetComponent<IHealth>();
			health.Current = enemyData.HP;
			health.Max = enemyData.HP;
			
			enemy.GetComponent<ActorUI>().Construct(health);
			enemy.GetComponent<AgentMoveToHero>().Construct(HeroGameObject.transform);
			enemy.GetComponent<NavMeshAgent>().speed = enemyData.MoveSpeed;

			var attack = enemy.GetComponent<Attack>();
			attack.Construct(HeroGameObject.transform);
			attack.Damage = enemyData.Damage;
			attack.EffectiveDistance = enemyData.EffectiveDistance;
			attack.RadiusAttackHit = enemyData.RadiusAttackHit;

			enemy.GetComponent<RotateToHero>()?.Construct(HeroGameObject.transform);
			
			return enemy;
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
