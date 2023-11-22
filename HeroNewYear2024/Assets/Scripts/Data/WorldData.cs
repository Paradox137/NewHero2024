using System;

namespace HeroScripts.Data
{
	[Serializable]
	public class WorldData
	{
		public PositionOnLevel PositionOnLevel;

		public WorldData(string initialLevel)
		{
			PositionOnLevel = new PositionOnLevel(initialLevel);
		}
	}
	
	[Serializable]
	public class PositionOnLevel
	{
		public string Level;
		public Vector3Data Position;

		public PositionOnLevel(string level, Vector3Data position)
		{
			Level = level;
			Position = position;
		}
		public PositionOnLevel(string initialLevel)
		{
			Level = initialLevel;
		}
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
