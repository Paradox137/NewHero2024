using UnityEngine;

namespace HeroScripts.Infrastructure
{
	public interface IGameFactory
	{
		GameObject CreateHero(GameObject at);
		void CreateHud();
	}
}
