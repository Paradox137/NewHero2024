using System;

namespace HeroScripts.Data
{
	[Serializable]
	public class PlayerProgress
	{
		public HeroState HeroState;
		public WorldData WorldData;
		public PlayerProgress(string initialLevel)
		{
			WorldData = new WorldData(initialLevel);
			
			HeroState = new HeroState();
		}
	}

}
