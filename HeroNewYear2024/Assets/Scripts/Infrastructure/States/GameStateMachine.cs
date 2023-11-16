using System;
using System.Collections.Generic;
using HeroScripts.Infrastructure.Services;
using HeroScripts.Logic;

namespace HeroScripts.Infrastructure
{
	public class GameStateMachine
	{
		private readonly Dictionary<Type, IExitableState> _states;
		private IExitableState _activeState;

		public GameStateMachine(SceneLoader __sceneLoader, LoadingCurtain __loadingCurtain, AllServices __allServices)
		{
			_states = new Dictionary<Type, IExitableState>()
			{
				[typeof(BootstrapState)] = new BootstrapState(this, __sceneLoader, __allServices),
				[typeof(LoadLevelState)] = new LoadLevelState(this, __sceneLoader, __loadingCurtain),
				[typeof(GameLoopState)] = new GameLoopState(this),
			};
		}
		public void Enter<TState>() where TState : class, IState
		{
			IState state = ChangeState<TState>();

			state.Enter();
		}
		public void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadedState<TPayload>
		{
			TState state = ChangeState<TState>();

			state.Enter(payload);
		}

		private TState ChangeState<TState>() where TState : class, IExitableState
		{
			_activeState?.Exit();
			
			TState state = GetState<TState>();
			_activeState = state;

			return state;
		}
		
		private TState GetState<TState>() where TState : class, IExitableState 
		{
			return _states[typeof(TState)] as TState;
		}
	}
}
