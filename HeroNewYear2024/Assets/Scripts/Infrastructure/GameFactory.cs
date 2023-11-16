using HeroScripts.Infrastructure.AssetManagement;
using UnityEngine;

namespace HeroScripts.Infrastructure
{
	public class GameFactory : IGameFactory
	{

		private readonly AssetProvider _assetProvider;
		public GameFactory(AssetProvider assetProvider)
		{
			_assetProvider = assetProvider;
		}
		public GameObject CreateHero(GameObject at)
		{
			return _assetProvider.Instantiate(AssetsPath.HeroPath, at.transform.position);
		}
		
		public void CreateHud()
		{
			_assetProvider.Instantiate(AssetsPath.HudPath);
		}
		
	}

}
