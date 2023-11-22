using HeroScripts.Data;

namespace HeroScripts.Infrastructure.Services.PersistentProgress
{
	public interface IPersistentProgressService : IService
	{
		PlayerProgress Progress { get; set; }
	}
}
