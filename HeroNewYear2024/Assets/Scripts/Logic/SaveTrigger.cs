using System;
using HeroScripts.Infrastructure.Services;
using HeroScripts.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace HeroScripts.Logic
{
	[RequireComponent(typeof(BoxCollider))]
	public class SaveTrigger : MonoBehaviour
	{
		private ISaveLoadService _saveLoadService;
		private void Awake()
		{
			_saveLoadService = AllServices.Container.Single<ISaveLoadService>();
		}
		private void OnTriggerEnter(Collider other)
		{
			_saveLoadService.SaveProgress();

			Debug.Log("Progress Saved.");
			gameObject.SetActive(false);
		}

		private void OnDrawGizmos()
		{
			BoxCollider _collider = gameObject.GetComponent<BoxCollider>();
			
			Gizmos.color = new Color32(30, 200, 30, 130);
			
			Gizmos.DrawCube(transform.position + _collider.center, _collider.size);
		}
	}
}
