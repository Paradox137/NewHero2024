using System;

namespace HeroScripts.Data
{
	[Serializable]
	public class WorldData
	{
		public Vector3Data Position;
	}
	public class Vector3Data
	{
		public float X;
		public float Y;
		public float Z;

		public Vector3Data(float x, float y, float z)
		{
			X = x;
			Y = y;
			Z = z;
		}
	}
}
