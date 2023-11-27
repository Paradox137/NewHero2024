using System;

namespace HeroScripts.Data
{
	[Serializable]
	public class HeroState
	{
		public float CurrentHP;
		public float MaxHP;

		public void ResetHP() => CurrentHP = MaxHP;
	}
}
