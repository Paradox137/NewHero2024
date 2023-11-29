using System;
using System.Collections.Generic;
using HeroScripts.Infrastructure.Services;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace HeroScripts.Infrastructure
{
	public interface IGameFactory : IService
	{
		void Register(ISavedProgressReader progressReader);
		GameObject CreateHero(GameObject at);
		GameObject CreateHud();
		GameObject HeroGameObject { get; }
		event Action HeroCreated;
		List<ISavedProgressReader> ProgressReaders { get; }
		List<ISavedProgress> ProgressWriters { get; }
		void CleanUp();
	}
}
