using System;
using UnityEngine;

namespace HeroScripts.Enemy
{
	[RequireComponent(typeof(Attack))]
	public class CheckAttackRange : MonoBehaviour
	{
		public Attack Attack;
		public TriggerObserver TriggerObserver;

		private void Start()
		{
			TriggerObserver.TriggerEnter += TriggerEnter;
			TriggerObserver.TriggerExit += DisableAttack;
			
			Attack.DisableAttack();
		}
		private void DisableAttack(Collider obj)
		{
			Attack.DisableAttack();
		}
		private void TriggerEnter(Collider obj)
		{
			Attack.EnableAttack();
		}
	}
}
