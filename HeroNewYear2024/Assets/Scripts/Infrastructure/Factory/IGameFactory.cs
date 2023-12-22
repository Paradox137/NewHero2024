using System.Collections.Generic;
using System.Threading.Tasks;
using HeroScripts.Enemy;
using HeroScripts.Infrastructure.Services;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using HeroScripts.StaticData;
using UnityEngine;

namespace HeroScripts.Infrastructure
{
	public interface IGameFactory : IService
	{
		Task<GameObject> CreateHero(Vector3 at);
		Task<GameObject> CreateHud();
		List<ISavedProgressReader> ProgressReaders { get; }
		List<ISavedProgress> ProgressWriters { get; }
		void CleanUp();
		Task<GameObject> CreateEnemy(EnemyTypeID enemyTypeID, Transform parent);
		Task<LootEntity> CreateLoot();
		Task CreateSpawner(Vector3 at, string spawnerID, EnemyTypeID EnemyTypeID);
		Task CreateLevelTransfer(Vector3 at);
		Task WarmUp();
	}
}
