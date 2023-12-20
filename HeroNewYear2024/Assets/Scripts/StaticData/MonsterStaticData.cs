using UnityEngine;

namespace HeroScripts.StaticData
{
	[CreateAssetMenu(fileName = "MonsterData", menuName = "StaticData/Monster")]
	public class MonsterStaticData : ScriptableObject
	{
		public EnemyTypeID EnemyTypeID;
		
		[Range(1, 100)]
		public int HP = 50;

		[Range(1f, 30)]
		public float Damage = 10f;

		public int MinLoot;
		public int MaxLoot;
		
		[Range(0.5f, 1)]
		public float EffectiveDistance = 0.5f;
		
		[Range(0.5f, 1)]
		public float RadiusAttackHit = 0.5f;

		[Range(0,10)]
		public float MoveSpeed = 3;
		
		public GameObject prefab;
	}
}
