using HeroScripts.Enemy;
using UnityEngine;

namespace HeroScripts.Infrastructure.Services
{
	public class RandomService : IRandomService
	{
		public int Next(int min, int max) => Random.Range(min, max);
	}
}
