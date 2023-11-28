using UnityEngine;

namespace HeroScripts.Hero
{
	public class HeroAnimator : MonoBehaviour
	{
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private Animator _animator;
		
		private static readonly int MoveHash = Animator.StringToHash("Moving");
		private static readonly int IdleHash = Animator.StringToHash("Idle");
		private static readonly int HitHash = Animator.StringToHash("Hit");
		private static readonly int DieHash = Animator.StringToHash("Die");
		private void Update()
		{
			_animator.SetFloat(MoveHash, _characterController.velocity.magnitude, 0f, Time.deltaTime);
		}
		public void PlayHit()
		{
			_animator.SetTrigger(HitHash);
		}
		public void PlayDeath()
		{
			_animator.SetTrigger(DieHash);
		}
	}
}
