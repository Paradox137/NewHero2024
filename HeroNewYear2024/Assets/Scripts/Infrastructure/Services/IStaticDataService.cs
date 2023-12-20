using HeroScripts.Infrastructure.Services;

namespace HeroScripts.StaticData
{
	public interface IStaticDataService : IService
	{
		void Load();
		MonsterStaticData ForEnemy(EnemyTypeID typeID);
		LevelStaticData ForLevel(string sceneKey);
	}
}
