using System;
using HeroScripts.Data;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using HeroScripts.StaticData;
using UnityEngine;
using UnityEngine.Serialization;

namespace HeroScripts.Logic
{
	public class EnemySpawner : MonoBehaviour, ISavedProgress
	{
		public EnemyTypeID EnemyTypeID;
		private string _id;

		public bool Slain;
		
		private void Awake()
		{
			_id = GetComponent<UniqueID>().ID;
		}
		public void LoadProgress(PlayerProgress progress)
		{
			if (progress.KillData.ClearedSpawners.Contains(_id))
			{
				Slain = true;
			}
			else
			{
				Spawn();
			}
		}
		private void Spawn()
		{
			
		}
		public void UpdateProgress(PlayerProgress progress)
		{
			if(Slain)
				progress.KillData.ClearedSpawners.Add(_id);
		}
	}
}
