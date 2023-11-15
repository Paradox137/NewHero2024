using HeroScripts.Services.Input;

namespace HeroScripts.Infrastructure
{
	public class BootstrapState : IState
	{
		private const string Initial = "Initial";
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
		private void EnterLoadLevel()
		{
			 _stateMachine.Enter<LoadLevelState>();
		}
		public void Exit()
		{
			
		}

		private void RegisterService()
		{
			Game.InputService = new ComputerInputService();
		}
		
		private static IInputService RegisterInput()
		{
			/*if (Application.isMobilePlatform)
				InputService = new MobileInputService();
			else*/
			return new ComputerInputService();
		}
	}
}
