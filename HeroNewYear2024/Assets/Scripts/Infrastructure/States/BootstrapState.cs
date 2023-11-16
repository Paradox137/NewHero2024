using HeroScripts.Infrastructure.AssetManagement;
using HeroScripts.Infrastructure.Services;
using HeroScripts.Services.Input;

namespace HeroScripts.Infrastructure
{
	public class BootstrapState : IState
	{
		private const string Initial = "Initial";
		private const string Test = "test";
			
		private readonly GameStateMachine _stateMachine;
		private readonly SceneLoader _sceneLoader;
		private readonly AllServices _allServices;
		public BootstrapState(GameStateMachine __stateMachine, SceneLoader __sceneLoader, AllServices __allServices)
		{
			_stateMachine = __stateMachine;
			_sceneLoader = __sceneLoader;
			_allServices = __allServices;
		}
		public void Enter()
		{
			RegisterService();
			
			_sceneLoader.Load(Initial, EnterLoadLevel);
		}
		public void Exit()
		{
			
		}
		private void EnterLoadLevel()
		{
			 _stateMachine.Enter<LoadLevelState, string>(Test);
		}

		private void RegisterService()
		{
			_allServices.RegisterSingle<IInputService>(InputService());
			_allServices.RegisterSingle<IAssetsProvider>(new AssetsProvider());
			_allServices.RegisterSingle<IGameFactory>(new GameFactory(_allServices.Single<IAssetsProvider>()));
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
