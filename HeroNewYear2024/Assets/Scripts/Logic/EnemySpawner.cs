using System;
using System.Threading.Tasks;
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
	public class SpawnPoint : MonoBehaviour, ISavedProgress
	{
		public EnemyTypeID EnemyTypeID;
		public string ID { get; set; }

		public bool Slain;
		private IGameFactory _factory;
		private EnemyDeath _enemyDeath;

		public void Construct(IGameFactory factory)
		{
			_factory = factory;
		}
		
		public void LoadProgress(PlayerProgress progress)
		{
			if (!progress.KillData.ClearedSpawners.Contains(ID))
				Spawn();
		}
		private async void Spawn()
		{
			GameObject enemy = await _factory.CreateEnemy(EnemyTypeID, transform);
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
