using System;
using System.Collections.Generic;

namespace HeroScripts.Infrastructure
{
	public class GameStateMachine
	{
		private readonly Dictionary<Type, IState> _states;
		private IState _activeState;

		public GameStateMachine(SceneLoader __sceneLoader)
		{
			_states = new Dictionary<Type, IState>()
			{
				[typeof(BootstrapState)] = new BootstrapState(this, __sceneLoader),
				[typeof(LoadLevelState)] = new LoadLevelState(this, __sceneLoader),
			};
		}

		public void Enter<TState>() where TState : IState
		{
			_activeState?.Exit();

			IState state = _states[typeof(TState)];
			_activeState = state;

			state.Enter();
		}
		
	}
}
