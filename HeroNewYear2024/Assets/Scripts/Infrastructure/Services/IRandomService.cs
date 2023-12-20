using HeroScripts.Infrastructure.Services;

namespace HeroScripts.Enemy
{
	public interface IRandomService : IService
	{
		int Next(int minValue, int maxValue);
	}
}
