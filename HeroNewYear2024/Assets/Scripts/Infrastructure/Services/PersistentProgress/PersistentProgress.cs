using HeroScripts.Data;

namespace HeroScripts.Infrastructure.Services.PersistentProgress
{
	public class PersistentProgress : IPersistentProgressService
	{
		public PlayerProgress Progress { get; set; }
	}
}
