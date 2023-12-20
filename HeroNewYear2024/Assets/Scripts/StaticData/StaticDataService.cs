using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace HeroScripts.StaticData
{
	public class StaticDataService : IStaticDataService
	{
		private Dictionary<EnemyTypeID,MonsterStaticData> _enemies;
		
		public void LoadEnemies()
		{
			_enemies = Resources.LoadAll<MonsterStaticData>("StaticData/Monsters")
				.ToDictionary(x => x.EnemyTypeID, x => x);
		}

		public MonsterStaticData ForEnemy(EnemyTypeID typeID)
		{
			return _enemies.TryGetValue(typeID, out MonsterStaticData staticData) ? staticData : null;
		}
	}
}
