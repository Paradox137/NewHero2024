using System;
using System.Collections.Generic;
using HeroScripts.Infrastructure.AssetManagement;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace HeroScripts.Infrastructure
{
	public class GameFactory : IGameFactory
	{
		private readonly IAssetsProvider _assetProvider;

		public List<ISavedProgressReader> ProgressReaders { get; } = new List<ISavedProgressReader>();
		public List<ISavedProgress> ProgressWriters { get; } = new List<ISavedProgress>();
		
		public GameFactory(IAssetsProvider assetProvider)
		{
			_assetProvider = assetProvider;
		}
		public GameObject CreateHero(GameObject at)
		{
			GameObject hero =InstantiateRegistered(AssetsPath.HeroPath, at.transform.position);
			HeroGameObject = hero;
			HeroCreated?.Invoke();
			return hero;
		}
		
		public GameObject CreateHud()
		{
			return InstantiateRegistered(AssetsPath.HudPath);
		}
		public GameObject HeroGameObject { get; private set; }
		public event Action HeroCreated;

		public void CleanUp()
		{
			ProgressReaders.Clear();
			ProgressWriters.Clear();
		}
		private void Register(ISavedProgressReader progressReader)
		{
			if (progressReader is ISavedProgress progressWriter)
				ProgressWriters.Add(progressWriter);
			
			ProgressReaders.Add(progressReader);
		}

		private GameObject InstantiateRegistered(string prefabPath, Vector3 position)
		{
			GameObject gameObject = _assetProvider.Instantiate(prefabPath, position);

			foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
				Register(progressReader);
			
			return gameObject;
		}
		private GameObject InstantiateRegistered(string prefabPath)
		{
			GameObject gameObject = _assetProvider.Instantiate(prefabPath);

			foreach (ISavedProgressReader progressReader in gameObject.GetComponentsInChildren<ISavedProgressReader>())
				Register(progressReader);
			
			return gameObject;
		}
	}
}
