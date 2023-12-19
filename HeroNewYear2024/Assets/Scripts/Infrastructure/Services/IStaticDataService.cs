using HeroScripts.Infrastructure.Services;

namespace HeroScripts.StaticData
{
	public interface IStaticDataService : IService
	{
		void LoadEnemies();
		MonsterStaticData ForEnemy(EnemyTypeID typeID);
	}
}
