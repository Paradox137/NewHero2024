using HeroScripts.Data;
using UnityEngine;

namespace HeroScripts.Infrastructure.Services.SaveLoad
{
	public class SaveLoadService : ISavedLoadService
	{
		private const string Progress = "Progress";

		public void SaveProgress()
		{
			
		}
		public PlayerProgress LoadProgress()
		{
			return PlayerPrefs.GetString(Progress)?.ToDeserializedFromJSON<PlayerProgress>();
		}
	}
}
