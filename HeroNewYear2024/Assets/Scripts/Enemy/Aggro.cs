using System;
using System.Collections;
using UnityEngine;

namespace HeroScripts.Enemy
{
	public class Aggro : MonoBehaviour
	{
		public TriggerObserver TriggerObserver;
		public AgentMoveToHero Follow;
		public float Cooldown;
		
		private Coroutine _aggroCoroutine;
		private bool _hasAggroTarget;
		private void Start()
		{
			SwitchFollowOff();
			
			TriggerObserver.TriggerEnter += TriggerEnter;
			TriggerObserver.TriggerExit += TriggerExit;
		}
		private void TriggerExit(Collider obj)
		{
			if (!_hasAggroTarget)
				return;
			
			_hasAggroTarget = false;
				
			_aggroCoroutine = StartCoroutine(SwitchFollowOffAfterCooldown());
		}
		private void TriggerEnter(Collider obj)
		{
			if (_hasAggroTarget)
				return;
			
			_hasAggroTarget = true;
				
			StopAggroCoroutine();

			SwitchFollowOn();
		}
		private void SwitchFollowOn() => Follow.enabled = true;
		private void SwitchFollowOff() => Follow.enabled = false;
		
		private IEnumerator SwitchFollowOffAfterCooldown()
		{
			yield return new WaitForSeconds(Cooldown);
			
			SwitchFollowOff();
		}

		private void StopAggroCoroutine()
		{
			if (_aggroCoroutine != null)
			{
				StopCoroutine(_aggroCoroutine);

				_aggroCoroutine = null;
			}
		}
	}
}
