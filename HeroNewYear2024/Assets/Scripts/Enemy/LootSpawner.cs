using System;
using HeroScripts.Data;
using HeroScripts.Infrastructure;
using HeroScripts.Logic;
using TMPro;
using UnityEngine;

namespace HeroScripts.Enemy
{
	public class LootSpawner : MonoBehaviour
	{
		public EnemyDeath EnemyDeath;
		private IGameFactory _factory;
		private IRandomService _random;
		
		private int _lootMin;
		private int _lootMax;

		public void Construct(IGameFactory factory, IRandomService random)
		{
			_factory = factory;
			_random = random;
		}
		
		private void Start()
		{
			EnemyDeath.Happened += SpawnLoot;
		}

		private async void SpawnLoot()
		{
			EnemyDeath.Happened -= SpawnLoot;
			
			LootEntity loot = await _factory.CreateLoot();
			loot.transform.position = transform.position;
			loot.GetComponent<UniqueID>().GenerateId();
			
			var lootItem = new Loot()
			{
				Value = _random.Next(_lootMin, _lootMax)
			};
			
			loot.Initialize(lootItem);
		}

		public void SetLoot(int min, int max)
		{
			_lootMin = min;
			_lootMax = max;
		}
	}
}
