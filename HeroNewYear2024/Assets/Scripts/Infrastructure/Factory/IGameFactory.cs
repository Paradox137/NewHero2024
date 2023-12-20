using System.Collections.Generic;
using HeroScripts.Enemy;
using HeroScripts.Infrastructure.Services;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using HeroScripts.StaticData;
using UnityEngine;

namespace HeroScripts.Infrastructure
{
	public interface IGameFactory : IService
	{
		void Register(ISavedProgressReader progressReader);
		GameObject CreateHero(GameObject at);
		GameObject CreateHud();
		List<ISavedProgressReader> ProgressReaders { get; }
		List<ISavedProgress> ProgressWriters { get; }
		void CleanUp();
		GameObject CreateEnemy(EnemyTypeID enemyTypeID, Transform parent);
		LootEntity CreateLoot();
	}
}
