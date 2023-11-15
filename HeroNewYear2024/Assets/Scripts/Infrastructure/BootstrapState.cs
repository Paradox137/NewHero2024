using HeroScripts.Services.Input;

namespace HeroScripts.Infrastructure
{
	public class BootstrapState : IState
	{
		private readonly GameStateMachine _stateMachine;
		public BootstrapState(GameStateMachine __stateMachine)
		{
			_stateMachine = __stateMachine;
		}
		public void Enter()
		{
			RegisterService();
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
