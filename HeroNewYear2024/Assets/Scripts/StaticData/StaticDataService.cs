using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HeroScripts.StaticData
{
	public class StaticDataService : IStaticDataService
	{
		private Dictionary<EnemyTypeID, MonsterStaticData> _enemies;
		private Dictionary<string, LevelStaticData> _levels;

		public void Load()
		{
			_enemies = Resources.LoadAll<MonsterStaticData>("StaticData/Monsters")
				.ToDictionary(x => x.EnemyTypeID, x => x);
			
			_levels = Resources.LoadAll<LevelStaticData>("StaticData/Levels")
				.ToDictionary(x => x.LevelKey, x => x);
		}

		public MonsterStaticData ForEnemy(EnemyTypeID typeID)
		{
			return _enemies.TryGetValue(typeID, out MonsterStaticData staticData) ? staticData : null;
		}
		public LevelStaticData ForLevel(string sceneKey)
		{
			return _levels.TryGetValue(sceneKey, out LevelStaticData staticData) ? staticData : null;
		}
	}
}
