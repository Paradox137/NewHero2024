using System;

namespace HeroScripts.Data
{
	[Serializable]
	public class LootEntityData
	{
		public Vector3Data Position;
		public Loot Loot;

		public LootEntityData(Vector3Data position, Loot loot)
		{
			Position = position;
			Loot = loot;
		}
	}
}
