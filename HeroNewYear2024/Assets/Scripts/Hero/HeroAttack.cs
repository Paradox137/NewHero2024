using HeroScripts.Data;
using HeroScripts.Enemy;
using HeroScripts.Infrastructure.Services;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using HeroScripts.Logic;
using HeroScripts.Services.Input;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using Stats = HeroScripts.Data.Stats;

namespace HeroScripts.Hero
{
	[RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
	public class HeroAttack : MonoBehaviour, ISavedProgressReader
	{
		public HeroAnimator Animator;
		public CharacterController CharacterController;
		private Stats _stats;

		private IInputService _inputService;

		private static int _layerMask;
		private Collider[] _hits = new Collider[3];
		
		private void Awake()
		{
			_inputService = AllServices.Container.Single<IInputService>();

			_layerMask = 1 << LayerMask.NameToLayer("Hittable");
		}

		private void Update()
		{
			if (_inputService.IsAttackButtonUp() && !Animator.IsAttacking)
				Animator.PlayAttackNormal();
		}
		
		private void OnAttack()
		{
			PhysicsDebug.DrawDebug(StartPoint() + transform.forward, _stats.DamageRadius, 1.0f);
			for (int i = 0; i < Hit(); ++i)
			{
				_hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
			}
		}
		
		private int Hit() => Physics.OverlapSphereNonAlloc(StartPoint() + transform.forward, _stats.DamageRadius, _hits, _layerMask);
		public void LoadProgress(PlayerProgress progress)
		{
			_stats = progress.HeroStats;
		}

		private Vector3 StartPoint() => new Vector3(transform.position.x, CharacterController.center.y / 2, transform.position.z);
	}
}
