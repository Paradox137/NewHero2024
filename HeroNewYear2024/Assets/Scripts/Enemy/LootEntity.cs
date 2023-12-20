using System;
using System.Collections;
using HeroScripts.Data;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using HeroScripts.Logic;
using TMPro;
using UnityEngine;

namespace HeroScripts.Enemy
{
	public class LootEntity : MonoBehaviour, ISavedProgress
	{
		public GameObject Skull;
		public GameObject PickupFxPrefab;
		public TextMeshPro LootText;
		public GameObject PickupPopup;
		
		private Loot _loot;
		private bool _picked = false;
		private WorldData _worldData;

		private string _id;
		public void Construct(WorldData worldData)
		{
			_worldData = worldData;
		}

		private void Start()
		{
			_id = GetComponent<UniqueID>().ID;
		}

		public void Initialize(Loot loot)
		{
			_loot = loot;
		}

		private void OnTriggerEnter(Collider other)
		{
			PickUp();
		}
		
		private void PickUp()
		{
			if(_picked)
				return;
			
			_picked = true;

			UpdateWorldData();
			HideSkull();
			PlayPickupFx();
			ShowText();

			StartCoroutine(StartDestroyTimer());
		}
		private void UpdateWorldData()
		{
			_worldData.LootData.Collect(_loot);
			RemoveLootPieceFromSavedPieces();
		}
		private void HideSkull() => Skull.SetActive(false);
		private void PlayPickupFx() => Instantiate(PickupFxPrefab, transform.position, Quaternion.identity);

		private void ShowText()
		{
			LootText.text = $"{_loot.Value}";
			PickupPopup.SetActive(true);
		}
		
		private IEnumerator StartDestroyTimer()
		{
			yield return new WaitForSeconds(1.5f);
			
			Destroy(gameObject);
		}
		
		private void RemoveLootPieceFromSavedPieces()
		{
			LootEntityDataDictionary savedLootPieces = _worldData.LootData.LootEntitiesOnScene;

			if (savedLootPieces.Dictionary.ContainsKey(_id))
			{
				savedLootPieces.Dictionary.Remove(_id);
			}
		}
		
		public void LoadProgress(PlayerProgress progress)
		{
			
		}
		public void UpdateProgress(PlayerProgress progress)
		{
			if (_picked)
				return;

			LootEntityDataDictionary lootPiecesOnScene = progress.WorldData.LootData.LootEntitiesOnScene;

			if (!lootPiecesOnScene.Dictionary.ContainsKey(_id))
				progress.WorldData.LootData.LootEntitiesOnScene.Dictionary.Add(_id, new LootEntityData(transform.position.AsVector3Data(), _loot));
		}
		private void OnApplicationQuit()
		{
			Debug.Log(_worldData.LootData.LootEntitiesOnScene.Dictionary.Count);
		}
	}
}
