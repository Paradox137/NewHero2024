using HeroScripts.Data;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using HeroScripts.Infrastructure.Services.SaveLoad;

namespace HeroScripts.Infrastructure.States
{
	public class LoadProgressState : IState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly IPersistentProgressService _progressService;
		private readonly ISavedLoadService _saveLoadService;
		public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISavedLoadService savedLoadService)
		{
			_gameStateMachine = gameStateMachine;
			_progressService = progressService;
			_saveLoadService = savedLoadService;
		}

		public void Enter()
		{
			LoadProgressOrInitNew();
			_gameStateMachine.Enter<LoadLevelState, string>(_progressService.Progress.WorldData.PositionOnLevel.Level);
		}
		public void Exit()
		{
			
		}
		private void LoadProgressOrInitNew()
		{
			_progressService.Progress = _saveLoadService.LoadProgress() ?? NewProgress();
		}
		private PlayerProgress NewProgress() => new PlayerProgress("test");
	}
}
