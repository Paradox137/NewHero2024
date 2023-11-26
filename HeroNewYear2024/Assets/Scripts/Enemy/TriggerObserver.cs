using System;
using UnityEngine;

namespace HeroScripts.Enemy
{
	[RequireComponent(typeof(Collider))]
	public class TriggerObserver : MonoBehaviour
	{
		public event Action<Collider> TriggerEnter;
		public event Action<Collider> TriggerExit;

		private void OnTriggerEnter(Collider other)
		{
			Debug.Log("on");
			TriggerEnter?.Invoke(other);
		}

		private void OnTriggerExit(Collider other)
		{
			Debug.Log("off");
			TriggerExit?.Invoke(other);
		}
	}
}
