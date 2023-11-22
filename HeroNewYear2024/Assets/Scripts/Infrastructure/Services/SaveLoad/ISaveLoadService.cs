using HeroScripts.Data;

namespace HeroScripts.Infrastructure.Services.SaveLoad
{
	public interface ISaveLoadService : IService
	{
		void SaveProgress();
		PlayerProgress LoadProgress();
	}
}
