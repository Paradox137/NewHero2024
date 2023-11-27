using System;
using HeroScripts.Infrastructure;
using HeroScripts.Infrastructure.Services;
using UnityEngine;

namespace HeroScripts.Enemy
{
	[RequireComponent(typeof(EnemyAnimator))]
	public class Attack : MonoBehaviour
	{
		public EnemyAnimator Animator;
		public float AttackCooldown = 3f;
		
		private IGameFactory _factory;
		private Transform _heroTransform;
		private float _attackCooldown;
		private bool _isAttacking;

		private void Awake()
		{
			_factory = AllServices.Container.Single<IGameFactory>();
			_factory.HeroCreated += OnHeroCreated;
		}

		private void Update()
		{
			UpdateCooldown();

			if(CanAttack())
				StartAttack();
		}

		private void OnAttack()
		{
			
		}

		private void OnAttackEnded()
		{
			_attackCooldown = AttackCooldown;
			_isAttacking = true;
		}

		private bool CanAttack() => CooldownEnded() && !_isAttacking;
		private bool CooldownEnded() => _attackCooldown <= 0;
		private void OnHeroCreated() => _heroTransform = _factory.HeroGameObject.transform;
		private void UpdateCooldown()
		{
			if (!CooldownEnded())
				_attackCooldown -= Time.deltaTime;
		}

		private void StartAttack()
		{
			transform.LookAt(_heroTransform);
			
			Animator.PlayAttack();

			_isAttacking = true;
		}

	}
}
