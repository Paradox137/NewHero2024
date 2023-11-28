using System;
using HeroScripts.Hero;
using UnityEngine;

namespace HeroScripts.UI
{
	class ActorUI : MonoBehaviour
	{
		public HpBar HpBar;

		private HeroHealth _heroHealth;

		public void Construct(HeroHealth health)
		{
			_heroHealth = health;

			_heroHealth.HealthChanged += UpdateHpBar;
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
