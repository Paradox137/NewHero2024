using System;

namespace HeroScripts.Data
{
	[Serializable]
	public class LootData
	{
		public int Collected;
		//todo event
		public Action Changed;

		public void Collect(Loot loot)
		{
			Collected += loot.Value;
			Changed?.Invoke();
		}
	}
}
