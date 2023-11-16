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
		public BootstrapState(GameStateMachine __stateMachine, SceneLoader __sceneLoader)
		{
			_stateMachine = __stateMachine;
			_sceneLoader = __sceneLoader;
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
			AllServices.Container.RegisterSingle<IInputService>(InputService());
			AllServices.Container.RegisterSingle<IGameFactory>(new GameFactory(AllServices.Container.Single<IAssetsProvider>()));
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
