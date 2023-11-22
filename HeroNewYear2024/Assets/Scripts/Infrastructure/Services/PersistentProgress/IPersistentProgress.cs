using HeroScripts.Data;

namespace HeroScripts.Infrastructure.Services.PersistentProgress
{
	public interface IPersistentProgress : IService
	{
		PlayerProgress Progress { get; set; }
	}
}
