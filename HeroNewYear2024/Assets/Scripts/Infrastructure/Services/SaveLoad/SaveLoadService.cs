using HeroScripts.Data;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace HeroScripts.Infrastructure.Services.SaveLoad
{
	public class SaveLoadService : ISaveLoadService
	{
		private readonly IPersistentProgressService _progressService;
		private readonly IGameFactory _gameFactory;
		private const string ProgressKey = "Progress";
		
		public SaveLoadService(IPersistentProgressService progressService, IGameFactory gameFactory)
		{
			_progressService = progressService;
			_gameFactory = gameFactory;
		}
		
		public void SaveProgress()
		{
			foreach (ISavedProgress progressWriter in _gameFactory.ProgressWriters)
			{
				progressWriter.UpdateProgress(_progressService.Progress);
			}
			
			PlayerPrefs.SetString(ProgressKey, _progressService.Progress.ToJSON());
		}
		public PlayerProgress LoadProgress()
		{
			return PlayerPrefs.GetString(ProgressKey)?.ToDeserializedFromJSON<PlayerProgress>();
		}
	}
}
