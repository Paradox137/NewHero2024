using System;
using HeroScripts.Data;
using HeroScripts.Enemy;
using HeroScripts.Infrastructure;
using HeroScripts.Infrastructure.Services;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using HeroScripts.StaticData;
using UnityEngine;
using UnityEngine.Serialization;

namespace HeroScripts.Logic
{
	public class EnemySpawner : MonoBehaviour, ISavedProgress
	{
		public EnemyTypeID EnemyTypeID;
		public string ID { get; set; }

		public bool Slain;
		private IGameFactory _factory;
		private EnemyDeath _enemyDeath;

		private void Awake()
		{
			ID = GetComponent<UniqueID>().ID;

			_factory = AllServices.Container.Single<IGameFactory>();
		}
		public void LoadProgress(PlayerProgress progress)
		{
			if (progress.KillData.ClearedSpawners.Contains(ID))
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
			GameObject enemy = _factory.CreateEnemy(EnemyTypeID, transform);
			_enemyDeath = enemy.GetComponent<EnemyDeath>();
			_enemyDeath.Happened += Slay;
		}
		private void Slay()
		{
			if(_enemyDeath != null)
				_enemyDeath.Happened -= Slay;
			
			Slain = true;
		}
		public void UpdateProgress(PlayerProgress progress)
		{
			if(Slain)
				progress.KillData.ClearedSpawners.Add(ID);
		}
	}
}
