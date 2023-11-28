using System;

namespace HeroScripts.Data
{
	[Serializable]
	public class PlayerProgress
	{
		public HeroState HeroState;
		public Stats HeroStats;
		public WorldData WorldData;
		public PlayerProgress(string initialLevel)
		{
			WorldData = new WorldData(initialLevel);
			HeroState = new HeroState();
			HeroStats = new Stats();
		}
	}

}
