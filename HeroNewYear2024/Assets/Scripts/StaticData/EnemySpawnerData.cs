using System;
using UnityEngine;

namespace HeroScripts.StaticData
{
	[Serializable]
	public class EnemySpawnerData
	{
		public string ID;
		public EnemyTypeID EnemyTypeID;
		public Vector3 Position;
	}
}
