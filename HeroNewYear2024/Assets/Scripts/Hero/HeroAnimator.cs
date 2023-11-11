using UnityEngine;

namespace HeroScripts.Hero
{
	public class HeroAnimator : MonoBehaviour
	{
		[SerializeField] private CharacterController _characterController;
		[SerializeField] private Animator _animator;
		
		private static readonly int MoveHash = Animator.StringToHash("Moving");
		private static readonly int IdleHash = Animator.StringToHash("Idle");
		
		private void Update()
		{
			_animator.SetFloat(MoveHash, _characterController.velocity.magnitude, 0f, Time.deltaTime);
		}
	}
}
