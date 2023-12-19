using HeroScripts.Data;
using HeroScripts.Infrastructure.Services.PersistentProgress;
using HeroScripts.Infrastructure.Services.SaveLoad;

namespace HeroScripts.Infrastructure.States
{
	public class LoadProgressState : IState
	{
		private readonly GameStateMachine _gameStateMachine;
		private readonly IPersistentProgressService _progressService;
		private readonly ISaveLoadService _saveLoadService;
		public LoadProgressState(GameStateMachine gameStateMachine, IPersistentProgressService progressService, ISaveLoadService savedLoadService)
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
		private PlayerProgress NewProgress()
		{
			PlayerProgress progress = new PlayerProgress("test");
			
			progress.HeroStats.Damage = 14;
			
			progress.HeroStats.DamageRadius = 0.5f;
			
			progress.HeroState.MaxHP = 50;
			progress.HeroState.ResetHP();

			return progress;			
		}
	}
}
