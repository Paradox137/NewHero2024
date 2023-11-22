using HeroScripts.Data;

namespace HeroScripts.Infrastructure.Services.SaveLoad
{
	public interface ISavedLoadService : IService
	{
		void SaveProgress();
		PlayerProgress LoadProgress();
	}
}
