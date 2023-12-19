using UnityEngine;

namespace HeroScripts.StaticData
{
	[CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
	public class MonsterStaticData : ScriptableObject
	{
		public EnemyTypeID EnemyTypeID;
		
		[Range(1, 100)]
		public int HP;

		[Range(1f, 30)]
		public float Damage;

		[Range(0.5f, 1)]
		public float EffectiveDistance;
		
		[Range(0.5f, 1)]
		public float RadiusAttackHit;

		public GameObject prefab;
	}
}
