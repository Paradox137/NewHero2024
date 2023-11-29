using System;
using HeroScripts.Hero;
using HeroScripts.Logic;
using UnityEngine;

namespace HeroScripts.UI
{
	class ActorUI : MonoBehaviour
	{
		public HpBar HpBar;

		private IHealth _health;

		public void Construct(IHealth health)
		{
			_health = health;

			_health.HealthChanged += UpdateHpBar;
		}

		private void OnEnable()
		{
			IHealth health = GetComponent<IHealth>();

			if (health != null)
			{
				Construct(health);
			}
		}
		private void OnDestroy()
		{
			_health.HealthChanged -= UpdateHpBar;
		}

		private void UpdateHpBar()
		{
			HpBar.SetValue(_health.Current, _health.Max);
		}

	}
}
