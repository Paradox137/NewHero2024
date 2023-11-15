namespace HeroScripts.Infrastructure
{
	public class LoadLevelState : IState
	{
		private const string Test = "test";
		private readonly GameStateMachine _gameStateMachine;
		private readonly SceneLoader _sceneLoader;

		public LoadLevelState(GameStateMachine gameStateMachine, SceneLoader sceneLoader)
		{
			_gameStateMachine = gameStateMachine;
			_sceneLoader = sceneLoader;

		}
			
		public void Enter()
		{
			_sceneLoader.Load(Test);
		}
		public void Exit()
		{
		}
	}
}
