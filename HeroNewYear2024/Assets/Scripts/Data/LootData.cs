using System;

namespace HeroScripts.Data
{
	[Serializable]
	public class LootData
	{
		public int Collected;
		public LootEntityDataDictionary LootEntitiesOnScene;

		public event Action Changed;
		public LootData()
		{
			LootEntitiesOnScene = new LootEntityDataDictionary();
		}
		public void Collect(Loot loot)
		{
			Collected += loot.Value;
			Changed?.Invoke();
		}
	}
}
