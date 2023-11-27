using System;
using System.Linq;
using HeroScripts.Infrastructure;
using HeroScripts.Infrastructure.Services;
using UnityEngine;

namespace HeroScripts.Enemy
{
	[RequireComponent(typeof(EnemyAnimator))]
	public class Attack : MonoBehaviour
	{
		[SerializeField] private EnemyAnimator Animator;
		[SerializeField] private float AttackCooldown = 3f;
		[SerializeField] private float RadiusAttackHit = 0.5f;
		[SerializeField] private float TestDistance = 0.5f;
		
		private IGameFactory _factory;
		private Transform _heroTransform;
		private float _attackCooldown;
		private bool _isAttacking;
		private int _layerMask;
		private Collider[] _hits = new Collider[1];
		private bool _attackIsActive;

		private void Awake()
		{
			_factory = AllServices.Container.Single<IGameFactory>();

			_layerMask = 1 << LayerMask.NameToLayer("Player");
			
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
			if (Hit(out Collider hit))
			{
				PhysicsDebug.DrawDebug(StartHitPoint() + transform.forward * TestDistance, RadiusAttackHit, 1f);
			}
		}

		private void OnAttackEnded()
		{
			_attackCooldown = AttackCooldown;
			_isAttacking = false;
		}
		
		private bool Hit(out Collider hit)
		{
			Vector3 startHitPoint = StartHitPoint();
			
			int hitCount = Physics.OverlapSphereNonAlloc(startHitPoint + transform.forward * TestDistance, RadiusAttackHit, _hits, _layerMask);

			hit = _hits.FirstOrDefault();

			return hitCount > 0;
		}

		private Vector3 StartHitPoint() => new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
		private bool CanAttack() => _attackIsActive && CooldownEnded() && !_isAttacking;
		private bool CooldownEnded() => _attackCooldown <= 0;
		private void OnHeroCreated() => _heroTransform = _factory.HeroGameObject.transform;
		public void DisableAttack() => _attackIsActive = false;
		public void EnableAttack() => _attackIsActive = true;
		
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
