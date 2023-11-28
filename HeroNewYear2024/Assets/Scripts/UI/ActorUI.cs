using System;
using HeroScripts.Hero;
using HeroScripts.Logic;
using UnityEngine;

namespace HeroScripts.UI
{
	class ActorUI : MonoBehaviour
	{
		public HpBar HpBar;

		private IHealth _heroHealth;

		public void Construct(IHealth health)
		{
			_heroHealth = health;

			_heroHealth.HealthChanged += UpdateHpBar;
		}

		private void Start()
		{
			IHealth health = GetComponent<IHealth>();

			if (health != null)
			{
				Construct(health);
			}
		}
		private void OnDestroy()
		{
			_heroHealth.HealthChanged -= UpdateHpBar;
		}

		private void UpdateHpBar()
		{
			HpBar.SetValue(_heroHealth.Current, _heroHealth.Max);
		}

	}
}
