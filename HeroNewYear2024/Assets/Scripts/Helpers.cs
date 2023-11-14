using UnityEngine;

namespace HeroScripts
{
	public static class Helpers
	{
		private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
		
		//todo: test matrix понимание заменить
		private static Matrix4x4 _isoMatrix2 = new Matrix4x4();
		public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint(input);
	}
}
