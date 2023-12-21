using System;
using System.Collections.Generic;
using HeroScripts.Infrastructure.Services;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using HeroScripts.Infrastructure.Services.SaveLoad;
using HeroScripts.Infrastructure.States;
using HeroScripts.Logic;
using HeroScripts.StaticData;

namespace HeroScripts.Infrastructure
{
	public class GameStateMachine : IGameStateMachine
	{
		private readonly Dictionary<Type, IExitableState> _states;
		private IExitableState _activeState;

		public GameStateMachine(SceneLoader __sceneLoader, LoadingCurtain __loadingCurtain, AllServices __allServices)
		{
			_states = new Dictionary<Type, IExitableState>()
			{
				[typeof(BootstrapState)] = new BootstrapState(
					this,
					__sceneLoader,
					__allServices),

				[typeof(LoadLevelState)] = new LoadLevelState(
					this,
					__sceneLoader,
					__loadingCurtain,
					__allServices.Single<IGameFactory>(),
					__allServices.Single<IPersistentProgressService>(),
					__allServices.Single<IStaticDataService>()),

				[typeof(LoadProgressState)] = new LoadProgressState(
					this,
					__allServices.Single<IPersistentProgressService>(),
					__allServices.Single<ISaveLoadService>()),

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
