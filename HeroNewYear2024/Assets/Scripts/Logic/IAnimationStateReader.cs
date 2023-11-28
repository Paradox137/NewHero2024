using HeroScripts.Hero;

namespace HeroScripts.Logic
{
	public interface IAnimationStateReader
	{
		void EnteredState(int stateHash);
		void ExitedState(int stateHash);
		AnimatorState State { get; }
	}
}
