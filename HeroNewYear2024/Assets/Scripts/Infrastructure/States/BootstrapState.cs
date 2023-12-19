using System.ComponentModel;
using HeroScripts.Infrastructure.AssetManagement;
using HeroScripts.Infrastructure.Services;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using HeroScripts.Infrastructure.Services.SaveLoad;
using HeroScripts.Infrastructure.States;
using HeroScripts.Services.Input;
using HeroScripts.StaticData;

namespace HeroScripts.Infrastructure
{
	public class BootstrapState : IState
	{
		private const string Initial = "Initial";

		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly AllServices _allServices;
		public BootstrapState(GameStateMachine __stateMachine, SceneLoader __sceneLoader, AllServices __allServices)
		{
			_stateMachine = __stateMachine;
			_sceneLoader = __sceneLoader;
			_allServices = __allServices;

			RegisterService();
		}
		public void Enter()
		{
			_sceneLoader.Load(Initial, EnterLoadLevel);
		}
		public void Exit()
		{
			
		}
		private void EnterLoadLevel()
		{
			_stateMachine.Enter<LoadProgressState>();
		}

		private void RegisterService()
		{
			_allServices.RegisterSingle<IInputService>(InputService());
			
			_allServices.RegisterSingle<IAssetsProvider>(new AssetsProvider());

			_allServices.RegisterSingle<IPersistentProgressService>(new PersistentProgress());

			_allServices.RegisterSingle<IGameFactory>(new GameFactory(_allServices.Single<IAssetsProvider>()));
			
			_allServices.RegisterSingle<ISaveLoadService>(new SaveLoadService(_allServices.Single<IPersistentProgressService>(), 
				_allServices.Single<IGameFactory>()));

			RegisterStaticData();
		}
		private void RegisterStaticData()
		{
			var staticData = new StaticDataService();
			staticData.LoadEnemies();
			_allServices.RegisterSingle(staticData);
		}

		private static IInputService InputService()
		{
			/*if (Application.isMobilePlatform)
				InputService = new MobileInputService();
			else*/
			return new ComputerInputService();
		}
	}
}
