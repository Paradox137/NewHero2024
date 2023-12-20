using System;
using HeroScripts.Data;
using UnityEngine;

namespace HeroScripts.Enemy
{
	public class LootEntity : MonoBehaviour
	{
		private Loot _loot;
		private bool _picked = false;
		private WorldData _worldData;

		public void Construct(WorldData worldData)
		{
			_worldData = worldData;
		}
		
		public void Initialize(Loot loot)
		{
			_loot = loot;
		}

		private void OnTriggerEnter(Collider other)
		{
			PickUp();
		}
		
		private void PickUp()
		{
			if(_picked)
				return;
			
			_picked = true;

			_worldData.LootData.Collect(_loot);
		}
	}
}
