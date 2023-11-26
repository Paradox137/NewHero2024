using System;
using UnityEngine;

namespace HeroScripts.Enemy
{
	public class Aggro : MonoBehaviour
	{
		public TriggerObserver TriggerObserver;
		public AgentMoveToHero Follow;

		private void Start()
		{
			SwitchFollowOff();
			
			TriggerObserver.TriggerEnter += TriggerEnter;
			TriggerObserver.TriggerExit += TriggerExit;
		}
		private void TriggerExit(Collider obj)
		{
			SwitchFollowOff();
		}
		private void TriggerEnter(Collider obj)
		{
			SwitchFollowOn();
		}
		private void SwitchFollowOn() => Follow.enabled = true;
		private void SwitchFollowOff() => Follow.enabled = false;
	}
}
