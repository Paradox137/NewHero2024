using HeroScripts.Data;

namespace HeroScripts.Infrastructure.Services.PersistentProgress
{
	public class PersistentProgress : IPersistentProgress
	{
		public PlayerProgress Progress { get; set; }
	}
}
