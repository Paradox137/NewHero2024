using HeroScripts.Data;
using UnityEngine;

namespace HeroScripts
{
	public static class DataExtensions
	{
		private static Matrix4x4 _isoMatrix = Matrix4x4.Rotate(Quaternion.Euler(0, 45, 0));
		public static Vector3 ToIso(this Vector3 input) => _isoMatrix.MultiplyPoint(input);
		public static Vector3Data AsVector3Data(this Vector3 vector3) => new Vector3Data(vector3.x, vector3.y, vector3.z);
	}
}
