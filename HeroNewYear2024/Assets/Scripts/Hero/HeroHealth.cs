using System;
using HeroScripts.Data;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace HeroScripts.Hero
{
	[RequireComponent(typeof(HeroAnimator))]
	public class HeroHealth : MonoBehaviour, ISavedProgress
	{
		public HeroAnimator Animator;
		private HeroState _state;

		public Action HealthChanged;
		public float Current
		{
			get => _state.CurrentHP;
			set
			{
				if (_state.CurrentHP != value)
				{
					_state.CurrentHP = value;
					
					HealthChanged?.Invoke();
				}
			}
		}
		public float Max
		{
			get => _state.MaxHP;
			set => _state.MaxHP = value;
		}
		public void LoadProgress(PlayerProgress progress)
		{
			_state = progress.HeroState;
		}
		public void UpdateProgress(PlayerProgress progress)
		{
			progress.HeroState.CurrentHP = Current;
			progress.HeroState.MaxHP = Max;
		}

		public void TakeDamage(float damage)
		{
			if (Current <= 0)
				return;

			Current -= damage;
			Animator.PlayHit();
		}
	}
}
