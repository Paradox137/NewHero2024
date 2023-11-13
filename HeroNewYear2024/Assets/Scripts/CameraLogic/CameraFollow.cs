using UnityEngine;

namespace HeroScripts.CameraLogic
{
	public class CameraFollow : MonoBehaviour
	{
		public Transform _following;
		Vector3 targetPos;
		public Vector3 offsetPos;
		public float moveSpeed = 5;
		public float smooth = 0.2f;
		private Vector3 velocity = Vector3.zero;
		private void LateUpdate()
		{
			MoveWithTarget();
		}

		public void Follow(GameObject following)
		{
			_following = following.transform;
		}

		void MoveWithTarget()
		{
			targetPos = _following.transform.position + offsetPos;
			//transform.position = Vector3.Lerp(transform.position, targetPos, moveSpeed * Time.deltaTime* smooth);
			transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref velocity, smooth);
		}
	}
}
